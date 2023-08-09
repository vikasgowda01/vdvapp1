export interface Pagination{
    currentPage: number;
    itemsPerPage: number;
    totalItems: number;
    totalPages: number;

}

export class PagniatedResult<T> {
    result?: T;
    pagination?: Pagination;
}