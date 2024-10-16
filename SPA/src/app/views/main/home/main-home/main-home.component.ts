import { Component, OnInit } from '@angular/core';
import { KeyValuePair } from '@utilities/key-value-pair';
import { ChatLuongBefore, ChuyenThongTin, MainHomeDto, MainHomeParam } from '@models/home';
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
  attribute: string[] = []
  stand: KeyValuePair[] = []
  param: MainHomeParam = <MainHomeParam>{};

  dataBefore: ChatLuongBefore[] = []
  // chatLuongBefore: ChatLuongBefore[] = [];
  // dataKetQua: ChuyenThongTin = <ChuyenThongTin>{};


  constructor(private service: HomeMainService) { }

  ngOnInit() {
    this.getListPlayer();
    this.getListExersice();
    if (this.dataBefore.length == 0)
      this.dataBefore.push(<ChatLuongBefore>{
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
        console.log('this.exercise :', this.exercise);
      }
    })
  }

  onSelect() {
    this.getData();
  }

  getData() {
    this.service.getData(this.param).subscribe({
      next: res => {
        this.data = res
        this.dataBefore = this.data.chatLuongBefore
        console.log('this.dataBefore :', this.dataBefore);
        this.chuyenThongTin();
      }
    })
  }
  addItem() {
    this.dataBefore.push(<ChatLuongBefore>{
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
  removeItem(i: number) {
    this.dataBefore = this.dataBefore.filter(item => item.id !== this.dataBefore[i].id);
    for (let i = 0; i < this.dataBefore.length; i++) {
      this.dataBefore[i].id = i + 1;
    }
  }

  changeBaiTap(index: number, baiTap: ChatLuongBefore) {
    this.getListThuocTinh(index, baiTap);

  }
  changeDiemTB(index: number, baiTap: ChatLuongBefore) {
    console.log('baiTap :', baiTap);
    this.dataBefore[index].canPha = null
    this.dataBefore[index].kemNguoi = null
    this.dataBefore[index].chayCho = null
    this.dataBefore[index].danhDau = null
    this.dataBefore[index].dungCam = null
    this.dataBefore[index].chuyenBong = null
    this.dataBefore[index].reBong = null
    this.dataBefore[index].tatCanh = null
    this.dataBefore[index].sutManh = null
    this.dataBefore[index].dutDiem = null
    this.dataBefore[index].theLuc = null
    this.dataBefore[index].sucManh = null
    this.dataBefore[index].xongXao = null
    this.dataBefore[index].tocDo = null
    this.dataBefore[index].sangTao = null
    this.getListThuocTinh(index, baiTap);
    // if()
  }

  getListThuocTinh(index: number, baiTap: ChatLuongBefore) {
    // let parseInt = +baiTap.split('-')[0]
    this.service.getListThuocTinh(baiTap.idBaiTap).subscribe({
      next: res => {
        this.attribute = res
        if (this.attribute && this.attribute.includes('CanPha')) {
          this.dataBefore[index].canPha = this.dataBefore[index].canPha + this.dataBefore[index].diemTB;
        }
      }
    })
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
    this.getListDisable();
  }

  getListDisable() {
    this.service.getListDisable(this.data.viTri).subscribe({
      next: res => {
        this.stand = res
        console.log('this.stand  :', this.stand);
      }
    })
  }
}
