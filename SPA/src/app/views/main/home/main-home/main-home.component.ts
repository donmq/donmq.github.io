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
  attributeDisable: KeyValuePair[] = []
  stand: KeyValuePair[] = []
  param: MainHomeParam = <MainHomeParam>{
    ten: "1"
  };

  dataBefore: ChatLuongBefore[] = []
  // chatLuongBefore: ChatLuongBefore[] = [];
  // dataKetQua: ChuyenThongTin = <ChuyenThongTin>{};


  constructor(private service: HomeMainService) { }

  ngOnInit() {
    this.getData();
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
    this.clear(index);
    this.getListThuocTinh(index, baiTap);

  }
  changeDiemTB(index: number, baiTap: ChatLuongBefore) {
    this.clear(index);
    this.getListThuocTinh(index, baiTap);
    // if()
  }

  clear(index: number) {
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
  }

  ketQua(index: number) {
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
  }
  getListThuocTinh(index: number, baiTap: ChatLuongBefore) {
    this.service.getListThuocTinh(baiTap.idBaiTap).subscribe({
      next: res => {
        this.attributeDisable = res
        console.log('this.attributeDisable :', this.attributeDisable);
        this.attribute = this.attributeDisable.map(x => x.key)
        console.log('this.att :', this.attribute);


        let soDiemCong = this.attribute.length * this.dataBefore[index].diemTB
        var diemChia = 100;

        const countValueOne = this.attributeDisable.filter(x => x.value === "1").length;

        if(this.attributeDisable.length == 5 && countValueOne == 1)
          diemChia = 33
        else if(this.attributeDisable.length == 5 && countValueOne == 2)
          diemChia = 57
        else if(this.attributeDisable.length == 5 && countValueOne == 3)
          diemChia = 75
        else if(this.attributeDisable.length == 5 && countValueOne == 4)
          diemChia = 89
        else if(this.attributeDisable.length == 4 && countValueOne == 1)
          diemChia = 40
        else if(this.attributeDisable.length == 4 && countValueOne == 2)
          diemChia = 67
        else if(this.attributeDisable.length == 4 && countValueOne == 3)
          diemChia = 86
        else if(this.attributeDisable.length == 3 && countValueOne == 1)
          diemChia = 50
        else if(this.attributeDisable.length == 3 && countValueOne == 2)
          diemChia = 80
        else if(this.attributeDisable.length == 2 && countValueOne == 1)
          diemChia = 67
        console.log('diemChia :', diemChia);

        let tongDiemToi = (soDiemCong / 100) * (100 - diemChia)
        let tongDiemSang = soDiemCong - tongDiemToi;


        let diemCongSang
        let diemCongToi

        if (countValueOne === 4) {
            diemCongSang = Math.floor(tongDiemSang / 4);
            diemCongToi = Math.floor(tongDiemToi);
        } else if (countValueOne === 3) {
            diemCongSang = Math.floor(tongDiemSang / 3);
            diemCongToi = Math.floor(tongDiemToi / 2);
        } else if (countValueOne === 2) {
            diemCongSang = Math.floor(tongDiemSang / 2);
            diemCongToi = Math.floor(tongDiemToi / 3);
        } else if (countValueOne === 1) {
            diemCongSang = Math.floor(tongDiemSang);
            diemCongToi = Math.floor(tongDiemToi / 4);
        }

        console.log('diemCongSang :', diemCongSang);
        console.log('diemCongToi :', diemCongToi);



        if(this.attribute.includes('CanPha') && this.attributeDisable){
           this.dataBefore[index].canPha = this.attributeDisable.find(x => x.key === 'CanPha')?.value === "1" ? diemCongSang : diemCongToi
           this.dataKetQua.canPha = this.dataBefore[index].canPha
          }
           console.log('this.dataKetQua.canPha :', this.dataKetQua.canPha);

        if (this.attribute.includes('KemNguoi')) {
           this.dataBefore[index].kemNguoi = this.attributeDisable.find(x => x.key === 'KemNguoi')?.value === "1" ? diemCongSang : diemCongToi
           this.dataKetQua.kemNguoi = Math.max(...this.dataBefore.map(x => x.kemNguoi));
          }
        if(this.attribute.includes('ChayCho')){
           this.dataBefore[index].chayCho = this.attributeDisable.find(x => x.key === 'ChayCho')?.value === "1" ? diemCongSang : diemCongToi
           this.dataKetQua.chayCho = Math.max(...this.dataBefore.map(x => x.chayCho));
          }
        if(this.attribute.includes('DanhDau')){
           this.dataBefore[index].danhDau = this.attributeDisable.find(x => x.key === 'DanhDau')?.value === "1" ? diemCongSang : diemCongToi
           this.dataKetQua.danhDau = Math.max(...this.dataBefore.map(x => x.danhDau));
          }
        if(this.attribute.includes('DungCam')){
           this.dataBefore[index].dungCam = this.attributeDisable.find(x => x.key === 'DungCam')?.value === "1" ? diemCongSang : diemCongToi
           this.dataKetQua.dungCam = Math.max(...this.dataBefore.map(x => x.dungCam));
          }
        if(this.attribute.includes('ChuyenBong')){
           this.dataBefore[index].chuyenBong = this.attributeDisable.find(x => x.key === 'ChuyenBong')?.value === "1" ? diemCongSang : diemCongToi
           this.dataKetQua.chuyenBong = Math.max(...this.dataBefore.map(x => x.chuyenBong));
          }
        if(this.attribute.includes('ReBong')){
           this.dataBefore[index].reBong = this.attributeDisable.find(x => x.key === 'ReBong')?.value === "1" ? diemCongSang : diemCongToi
           this.dataKetQua.reBong = Math.max(...this.dataBefore.map(x => x.reBong));
          }
        if(this.attribute.includes('TatCanh')){
           this.dataBefore[index].tatCanh = this.attributeDisable.find(x => x.key === 'TatCanh')?.value === "1" ? diemCongSang : diemCongToi
           this.dataKetQua.tatCanh = Math.max(...this.dataBefore.map(x => x.tatCanh));
          }
        if(this.attribute.includes('SutManh')){
           this.dataBefore[index].sutManh = this.attributeDisable.find(x => x.key === 'SutManh')?.value === "1" ? diemCongSang : diemCongToi
           this.dataKetQua.sutManh = Math.max(...this.dataBefore.map(x => x.sutManh));
          }
        if(this.attribute.includes('DutDiem')){
           this.dataBefore[index].dutDiem = this.attributeDisable.find(x => x.key === 'DutDiem')?.value === "1" ? diemCongSang : diemCongToi
           this.dataKetQua.dutDiem = Math.max(...this.dataBefore.map(x => x.dutDiem));
          }
        if(this.attribute.includes('TheLuc')){
           this.dataBefore[index].theLuc = this.attributeDisable.find(x => x.key === 'TheLuc')?.value === "1" ? diemCongSang : diemCongToi
           this.dataKetQua.theLuc = Math.max(...this.dataBefore.map(x => x.theLuc));
          }
        if(this.attribute.includes('SucManh')){
           this.dataBefore[index].sucManh = this.attributeDisable.find(x => x.key === 'SucManh')?.value === "1" ? diemCongSang : diemCongToi
           this.dataKetQua.sucManh = Math.max(...this.dataBefore.map(x => x.sucManh));
          }
        if(this.attribute.includes('XongXao')){
           this.dataBefore[index].xongXao = this.attributeDisable.find(x => x.key === 'XongXao')?.value === "1" ? diemCongSang : diemCongToi
           this.dataKetQua.xongXao = Math.max(...this.dataBefore.map(x => x.xongXao));
          }
        if(this.attribute.includes('TocDo')){
           this.dataBefore[index].tocDo = this.attributeDisable.find(x => x.key === 'TocDo')?.value === "1" ? diemCongSang : diemCongToi
           this.dataKetQua.tocDo = Math.max(...this.dataBefore.map(x => x.tocDo));
          }
        if(this.attribute.includes('SangTao')){
           this.dataBefore[index].sangTao = this.attributeDisable.find(x => x.key === 'SangTao')?.value === "1" ? diemCongSang : diemCongToi
           this.dataKetQua.sangTao = Math.max(...this.dataBefore.map(x => x.sangTao));
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

    this.dataKetQua.canPha = this.data.canPha + this.dataBefore.map(x => x.canPha).reduce((a,b) => a+b)
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
      }
    })
  }
}
