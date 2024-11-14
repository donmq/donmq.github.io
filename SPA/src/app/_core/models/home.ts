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
  qualityAfter: Quality[];
}

export interface DataCreate {
  dataTable: Quality;
  dataAfter: Quality[];
}

export interface MainHomeParam {
  inforID: number;
  exerciseID: number;
  planID: number;
}

export interface Quality {
  inforID: number;
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
  chatLuongChung: number;
  planID: number;
  soDiemTap: number;
  exerciseID: number | null;
  canDelete?: boolean
}



