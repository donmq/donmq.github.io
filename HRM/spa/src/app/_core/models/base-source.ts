import { Pagination } from "@utilities/pagination-utility";

export interface BaseSource<T> {
    pagination: Pagination;
    model: T | null;
    data: T[]
}

export class ValidateResult {
    constructor(mess: string) {
        this.message = mess;
        this.isSuccess = false;
    }
    isSuccess: boolean;
    message?: string
}
