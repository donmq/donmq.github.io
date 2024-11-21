export interface MainHomeDto {
  id: number;
  name: string;
  position: string;
  personality: string;
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
  qualityAfter: QualityAfter[];
}

export interface DataCreate {
  dataTable: ChuyenThongTin;
  dataAfter: QualityAfter[];
}

export interface MainHomeParam {
  inforID: number;
  exerciseID: number;
}

export interface ChuyenThongTin {
  id: number;
  name: string;
  position: string;
  average: number;
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

export interface QualityAfter {
  id: number;
  inforID: number | null;
  exerciseID: number | null;
  average: number | null;
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
  soDiemTap: number;
}

