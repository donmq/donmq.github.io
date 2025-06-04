import { KeyValueUtility } from "@utilities/key-value-utility";
import { CommentArchive } from "@models/common/comment-archive";
import { PaginationResult } from "@utilities/pagination-utility";
import { LeaveData } from "../common/leave-data";

export interface SeaConfirmResolverDto {
  category_List: KeyValueUtility[];
  department_List: KeyValueUtility[];
  comment_List: CommentArchive[]
}

export interface SeaConfirmSearch {
  leaveData: PaginationResult<LeaveData>;
  countEachCategory: KeyValueUtility[];
}
