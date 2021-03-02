export interface PaginatedResult<T> {
  totalItems: number;
  items: T[];
}
