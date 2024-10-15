import { Component, OnInit } from '@angular/core';
import { KeyValuePair } from '@utilities/key-value-pair';
import { ChuyenThongTin, MainHomeDto, MainHomeParam } from '@models/home';
import { HomeMainService } from './../../../../_core/services/home-main.service';


@Component({
  selector: 'app-main-home',
  templateUrl: './main-home.component.html',
  styleUrl: './main-home.component.scss'
})
export class MainHomeComponent implements OnInit {
  data: MainHomeDto = <MainHomeDto>{};
  dataTable: ChuyenThongTin = <ChuyenThongTin>{};
  dataKetQua: ChuyenThongTin = <ChuyenThongTin>{};
  players: KeyValuePair[] = []
  exercise: KeyValuePair[] = []
  attribute: KeyValuePair[] = []
  stand: KeyValuePair[] = []
  param: MainHomeParam = <MainHomeParam>{};

  dataBefore: ChuyenThongTin[] = []

  constructor(private service: HomeMainService) { }

  ngOnInit() {
    this.getListPlayer();
    this.getListExersice();
    if (this.dataBefore.length == 0)
      this.dataBefore.push(<ChuyenThongTin>{
        diemTB: 180,
        canPha: null,
        kemNguoi: null,
        chayCho: null,
        danhDau: null,
        dungCam: null,
        chuyenBong: null,
        reBong: null,
        tatCanh: null,
        sutManh: null,
        dutDiem: null,
        theLuc: null,
        sucManh: null,
        xongXao: null,
        tocDo: null,
        sangTao: null
      });
  }

  getListPlayer() {
    this.service.getListPlayers().subscribe({
      next: res => {
        this.players = res
      }
    })
  }

  getListExersice() {
    this.service.getListExercise().subscribe({
      next: res => {
        this.exercise = res
      }
    })
  }

  onSelect() {
    this.getData()
  }

  getData() {
    this.service.getData(this.param).subscribe({
      next: res => {
        this.data = res
      }
    })
  }
  onAdd() {

  }

  changeBaiTap(index: number, baiTap: string) {

    let parseInt = +baiTap.split('-')[0]
    console.log('parseInt :', parseInt);
    this.service.getListThuocTinh(parseInt).subscribe({
      next: res => {
        this.attribute = res
      }
    })
    // this.dataBefore[index].diemTB = 180
  }
  changeDiemTB(index: number) {
    // this.dataBefore[index].diemTB = 180
  }

  chuyenThongTin() {
    this.dataTable.ten = this.data.ten
    this.dataTable.viTri = this.data.viTri
    this.dataTable.canPha = this.data.canPha
    this.dataTable.kemNguoi = this.data.kemNguoi
    this.dataTable.chayCho = this.data.chayCho
    this.dataTable.danhDau = this.data.danhDau
    this.dataTable.dungCam = this.data.dungCam
    this.dataTable.chuyenBong = this.data.chuyenBong
    this.dataTable.reBong = this.data.reBong
    this.dataTable.tatCanh = this.data.tatCanh
    this.dataTable.sutManh = this.data.sutManh
    this.dataTable.dutDiem = this.data.dutDiem
    this.dataTable.theLuc = this.data.theLuc
    this.dataTable.sucManh = this.data.sucManh
    this.dataTable.xongXao = this.data.xongXao
    this.dataTable.tocDo = this.data.tocDo
    this.dataTable.sangTao = this.data.sangTao

    this.dataKetQua.canPha = this.data.canPha
    this.dataKetQua.kemNguoi = this.data.kemNguoi
    this.dataKetQua.chayCho = this.data.chayCho
    this.dataKetQua.danhDau = this.data.danhDau
    this.dataKetQua.dungCam = this.data.dungCam
    this.dataKetQua.chuyenBong = this.data.chuyenBong
    this.dataKetQua.reBong = this.data.reBong
    this.dataKetQua.tatCanh = this.data.tatCanh
    this.dataKetQua.sutManh = this.data.sutManh
    this.dataKetQua.dutDiem = this.data.dutDiem
    this.dataKetQua.theLuc = this.data.theLuc
    this.dataKetQua.sucManh = this.data.sucManh
    this.dataKetQua.xongXao = this.data.xongXao
    this.dataKetQua.tocDo = this.data.tocDo
    this.dataKetQua.sangTao = this.data.sangTao

    this.service.getListDisable(this.data.viTri).subscribe({
      next: res => {
        this.stand = res
        console.log('this.stand  :', this.stand );
      }
    })

  }

}
