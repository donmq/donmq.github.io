export interface MainHomeDto {
  id: number;
  ten: string;
  viTri: string;
  tuChat: string;
  canPha: number;
  kemNguoi: number;
  chayCho: number;
  danhDau: number;
  dungCam: number;
  chuyenBong: number;
  reBong: number;
  tatCanh: number;
  sutManh: number;
  dutDiem: number;
  theLuc: number;
  sucManh: number;
  xongXao: number;
  tocDo: number;
  sangTao: number;
  chatLuongBefore: ChatLuongBefore[];
}

export interface DataCreate {
  dataTable: ChuyenThongTin;
  dataBefore: ChatLuongBefore[];
}

export interface MainHomeParam {
  ten: string;
  baiTap: string;
}

export interface ChuyenThongTin {
  id: number;
  ten: string;
  viTri: string;
  diemTB: number;
  canPha: number | null;
  kemNguoi: number | null;
  chayCho: number | null;
  danhDau: number | null;
  dungCam: number | null;
  chuyenBong: number | null;
  reBong: number | null;
  tatCanh: number | null;
  sutManh: number | null;
  dutDiem: number | null;
  theLuc: number | null;
  sucManh: number | null;
  xongXao: number | null;
  tocDo: number | null;
  sangTao: number | null;
  chatLuongChung: string;
}

export interface ChatLuongBefore {
  id: number;
  idThongTin: number | null;
  idBaiTap: number | null;
  diemTB: number | null;
  canPha: number | null;
  kemNguoi: number | null;
  chayCho: number | null;
  danhDau: number | null;
  dungCam: number | null;
  chuyenBong: number | null;
  reBong: number | null;
  tatCanh: number | null;
  sutManh: number | null;
  dutDiem: number | null;
  theLuc: number | null;
  sucManh: number | null;
  xongXao: number | null;
  tocDo: number | null;
  sangTao: number | null;
  chatLuongChung: string;
}

