export interface CommentArchive {
  commentArchiveID: number;
  value: number | null;
  comment: string;
  description: string;
  userID: number | null;
  createTime: string;
  createName:string;
  text: string | null;
}