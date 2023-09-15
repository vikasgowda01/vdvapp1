import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { provideClientHydration } from '@angular/platform-browser';
import { ActivatedRoute } from '@angular/router';
import { NgxGalleryAnimation, NgxGalleryImage, NgxGalleryModule, NgxGalleryOptions } from '@kolkov/ngx-gallery';
import { Member } from 'src/app/_models/member';
import { MembersService } from 'src/app/_services/members.service';
import { MemberMessagesComponent } from '../member-messages/member-messages.component';
import { CommonModule } from '@angular/common';
import { TabDirective, TabsModule, TabsetComponent } from 'ngx-bootstrap/tabs';
import { TimeagoModule } from 'ngx-timeago';
import { MessageService } from 'src/app/_services/message.service';
import { Message } from 'src/app/_models/message';
import { PresenceService } from 'src/app/_services/presence.service';
import { AccountService } from 'src/app/_services/account.service';
import { User } from 'src/app/_models/user';
import { take } from 'rxjs';


@Component({
  selector: 'app-member-details',
  standalone: true,
  templateUrl: './member-details.component.html',
  styleUrls: ['./member-details.component.css'],
  imports:[CommonModule,TabsModule,TabsModule,NgxGalleryModule,TimeagoModule, MemberMessagesComponent]
})
export class MemberDetailsComponent implements OnInit, OnDestroy{
  @ViewChild('memberTabs', {static : true}) memberTabs?: TabsetComponent;
  member: Member = {} as Member;
  galleryOptions: NgxGalleryOptions[] =[];
  galleryImages: NgxGalleryImage[] =[];
  activeTab?: TabDirective;

  messages: Message[]=[];
  user?: User;

  constructor(private accountService: AccountService, private route: ActivatedRoute, 
    private messageService: MessageService, public presenceService: PresenceService) {
      this.accountService.currentUser$.pipe(take(1)).subscribe({
        next: user => {
          if(user) this.user = user;
        }
      })
    }
  
  ngOnInit(): void {
    this.route.data.subscribe({
      next: data => this.member =data['member']
    })
    this.route.queryParams.subscribe({
      next: params => {
        params['tab'] && this.selectTab(params['tab'])
      }
    })
    //this.galleryImages =
    this.getImages();
    this.galleryOptions =[
      {
        width:'500px',
        height: '500px',
        imagePercent: 100,
        thumbnailsColumns: 4,
        imageAnimation:NgxGalleryAnimation.Slide,
        preview:false
      }
    ]
    

  }
  ngOnDestroy(): void {
    this.messageService.stopHubConnection();
  }

  selectTab(heading: string) {
    if(this.memberTabs){
      this.memberTabs.tabs.find(x => x.heading === heading)!.active =true;
    }
  }
  onTabActivated(data: TabDirective) {
    this.activeTab = data;
    if(this.activeTab.heading === 'Messages' && this.user){
      this.messageService.createHubConnection(this.user, this.member.userName);
    }else {
      this.messageService.stopHubConnection();
    }
  }

  loadMessages(){
    if(this.member){
      this.messageService.getMessageThread(this.member.userName).subscribe({
        next: messages => this.messages = messages
      })
    }
   }
  getImages(){
    if(!this.member) return [];
    const imageUrls=[];
    for(const photo of this.member.photos){
      imageUrls.push({
        small:photo.url,
        medium:photo.url,
        big: photo.url
      })
    }
    return imageUrls;
  }


}
