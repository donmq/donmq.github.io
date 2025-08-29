export interface DirectoryInfomation {
  seq: string;
  directory_Name: string;
  directory_Code: string;
}
export interface ProgramInfomation {
  program_Name: string;
  program_Code: string;
  parent_Directory_Code: string;
}
export interface FunctionInfomation {
  program_Code: string;
  function_Code: string;
}
export interface CodeInformation {
  code: string;
  name: string;
  kind: string;
  translations: CodeLang[];
}
export interface CodeLang {
  lang: string;
  name: string;
}
export interface UserLoginParam {
  username: string;
  password: string;
  factory: string;
  lang: string;
}
export interface UserForLogged {
  id: string;
  factory: string;
  account: string;
  name: string;
}
export interface AuthProgram {
  directories: DirectoryInfomation[];
  programs: ProgramInfomation[];
  functions: FunctionInfomation[];
}
export interface DataResponse {
  user: UserForLogged;
  code_Information: CodeInformation[];
}
export interface ResultResponse {
  data: DataResponse;
  token: string;
}
