import { PaginationResult } from '@utilities/pagination-utility';
import { LeaveData } from './../../common/leave-data';

export interface leaveDataHistory {
    leaveData: PaginationResult<LeaveData>;
    sumLeaveDay: number | null;
}