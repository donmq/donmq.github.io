import { LeaveDataHistory } from "@params/seahr/seahr-history/leavedata-history";
import { KeyValueUtility } from "@utilities/key-value-utility";
import { PaginationResult } from "@utilities/pagination-utility";

export interface SeaHistorySearch {
  leaveData: PaginationResult<LeaveDataHistory>;
  countEachCategory: KeyValueUtility[];
}
