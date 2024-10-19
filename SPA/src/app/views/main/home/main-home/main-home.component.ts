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

  keys = ['canPha', 'kemNguoi', 'chayCho', 'danhDau', 'dungCam', 'chuyenBong', 'reBong', 'tatCanh', 'sutManh', 'dutDiem', 'theLuc', 'sucManh', 'xongXao', 'tocDo', 'sangTao'];

  dataBefore: ChatLuongBefore[] = []

  constructor(private service: HomeMainService) { }

  ngOnInit() {
    this.getData();
    this.getListPlayer();
    this.getListExersice();
    if (this.dataBefore.length == 0)
      this.addItem();
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
        for (let index in this.dataBefore) {
          this.dataBefore[index].chatLuongChung = Math.floor(this.keys.reduce((total, key) => total + this.dataBefore[index][key], 0) / 15) + "%";
        }
      }
    })
  }

  //#region Tính toán
  tinhToan(index: number) {
    let totalSum = 0;

    let filteredData: any = {};
    let lowercaseAttribute = this.attribute.map(a => a.toLowerCase());

    Object.keys(this.dataKetQua).forEach(key => {
      let lowercaseKey = key.toLowerCase();
      if (lowercaseAttribute.includes(lowercaseKey)) {
        filteredData[key] = this.dataKetQua[key];
        totalSum += this.dataKetQua[key];
      }
    });
    let soDiemCong
    if (index == 0)
      soDiemCong = this.attribute.length * this.dataBefore[index].diemTB
    else
      soDiemCong = this.attribute.length * this.dataBefore[index].diemTB - totalSum


    var diemChia = 100;

    const countValueOne = this.attributeDisable.filter(x => x.value === "1").length;

    if (this.attributeDisable.length == 5 && countValueOne == 1)
      diemChia = 33
    else if (this.attributeDisable.length == 5 && countValueOne == 2)
      diemChia = 57
    else if (this.attributeDisable.length == 5 && countValueOne == 3)
      diemChia = 75
    else if (this.attributeDisable.length == 5 && countValueOne == 4)
      diemChia = 89
    else if (this.attributeDisable.length == 4 && countValueOne == 1)
      diemChia = 40
    else if (this.attributeDisable.length == 4 && countValueOne == 2)
      diemChia = 67
    else if (this.attributeDisable.length == 4 && countValueOne == 3)
      diemChia = 86
    else if (this.attributeDisable.length == 3 && countValueOne == 1)
      diemChia = 50
    else if (this.attributeDisable.length == 3 && countValueOne == 2)
      diemChia = 80
    else if (this.attributeDisable.length == 2 && countValueOne == 1)
      diemChia = 67

    let tongDiemToi = (soDiemCong / 100) * (100 - diemChia)
    let tongDiemSang = soDiemCong - tongDiemToi;


    let diemCongSang
    let diemCongToi
    if (this.attributeDisable.length == 5) {
      if (countValueOne === 5) {
        diemCongSang = Math.ceil(tongDiemSang / 5);
        diemCongToi = 0
      } else if (countValueOne === 4) {
        diemCongSang = Math.ceil(tongDiemSang / 4);
        diemCongToi = Math.ceil(tongDiemToi);
      } else if (countValueOne === 3) {
        diemCongSang = Math.ceil(tongDiemSang / 3);
        diemCongToi = Math.ceil(tongDiemToi / 2);
      } else if (countValueOne === 2) {
        diemCongSang = Math.ceil(tongDiemSang / 2);
        diemCongToi = Math.ceil(tongDiemToi / 3);
      } else if (countValueOne === 1) {
        diemCongSang = Math.ceil(tongDiemSang);
        diemCongToi = Math.ceil(tongDiemToi / 4);
      }
    }
    if (this.attributeDisable.length == 4) {
      if (countValueOne === 4) {
        diemCongSang = Math.ceil(tongDiemSang / 4);
        diemCongToi = 0
      } else if (countValueOne === 3) {
        diemCongSang = Math.ceil(tongDiemSang / 3);
        diemCongToi = Math.ceil(tongDiemToi);
      } else if (countValueOne === 2) {
        diemCongSang = Math.ceil(tongDiemSang / 2);
        diemCongToi = Math.ceil(tongDiemToi / 2);
      } else if (countValueOne === 1) {
        diemCongSang = Math.ceil(tongDiemSang);
        diemCongToi = Math.ceil(tongDiemToi / 3);
      }
    }
    if (this.attributeDisable.length == 3) {
      if (countValueOne === 3) {
        diemCongSang = Math.ceil(tongDiemSang / 3);
        diemCongToi = 0
      } else if (countValueOne === 2) {
        diemCongSang = Math.ceil(tongDiemSang / 2);
        diemCongToi = Math.ceil(tongDiemToi);
      } else if (countValueOne === 1) {
        diemCongSang = Math.ceil(tongDiemSang);
        diemCongToi = Math.ceil(tongDiemToi / 2);
      }
    }
    if (this.attributeDisable.length == 2) {
      if (countValueOne === 2) {
        diemCongSang = Math.ceil(tongDiemSang / 2);
        diemCongToi = 0
      } else {
        diemCongSang = Math.ceil(tongDiemSang);
        diemCongToi = Math.ceil(tongDiemToi);
      }
    }
    debugger
    this.keys.forEach(key => {
      if (index === 0) {
        this.dataBefore[index][key] = (this.attribute.includes(key) ? (this.attributeDisable.find(x => x.key === key)?.value === "1" ? diemCongSang : diemCongToi) : 0);

      } else {
        this.dataBefore[index][key] = this.dataKetQua[key] + (this.attribute.includes(key) ? (this.attributeDisable.find(x => x.key === key)?.value === "1" ? diemCongSang : diemCongToi) : 0);
      }
      this.dataBefore.splice(index + 1);
      this.addItem();
      this.dataKetQua[key] = this.dataBefore[index][key];
    });

    this.dataBefore[index].chatLuongChung = Math.floor(this.keys.reduce((total, key) => total + this.dataBefore[index][key], 0) / 15) + "%";
    this.dataKetQua.chatLuongChung = Math.floor(this.keys.reduce((total, key) => total + this.dataKetQua[key], 0) / 15) + "%";

    console.log('this.dataBefore :', this.dataBefore);
  }
  //#endregion
  //#region Chuyển Thông tin
  chuyenThongTin() {

    this.dataTable.ten = this.data.ten
    this.dataTable.viTri = this.data.viTri

    this.keys.forEach(key => {
      this.dataTable[key] = this.data[key];
    });

    this.dataTable.chatLuongChung = Math.floor(this.keys.reduce((total, key) => total + this.data[key], 0) / 15) + "%";

    this.keys.forEach(key => {
      this.dataKetQua[key] = this.dataBefore.map(x => x[key]).reduce((a, b) => a + b) !== null ? this.dataBefore.map(x => x[key]).reduce((a, b) => a + b) : this.dataTable[key];
    });
    this.dataKetQua.chatLuongChung = Math.floor(this.keys.reduce((total, key) => total + this.dataKetQua[key], 0) / 15) + "%";

    this.getListDisable();
  }
  //#endregion

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
  removeItem(data: ChatLuongBefore) {
    // debugger
    this.dataBefore = this.dataBefore.filter(x => x.id != data.id)
  }

  changeBaiTap(index: number, baiTap: ChatLuongBefore) {
    this.clear(index);
    this.getListThuocTinh(index, baiTap);

  }
  changeDiemTB(index: number, baiTap: ChatLuongBefore) {
    this.clear(index);
    this.getListThuocTinh(index, baiTap);
  }

  clear(index: number) {
    this.keys.forEach(key => {
      this.dataBefore[index][key] = null
    });
  }

  ketQua(index: number) {
    // this.dataKetQua.canPha = this.dataBefore.map(x => x.canPha).reduce((a, b) => a + b)
    this.keys.forEach(key => {
      this.dataKetQua[key] = this.data[key]
    });
  }
  getListThuocTinh(index: number, baiTap: ChatLuongBefore) {
    this.service.getListThuocTinh(baiTap.idBaiTap, this.data.viTri).subscribe({
      next: res => {
        this.attributeDisable = res
        this.attribute = this.attributeDisable.map(x => x.key)

        this.tinhToan(index);

      }
    })
  }

  getListDisable() {
    this.service.getListDisable(this.data.viTri).subscribe({
      next: res => {
        this.stand = res
      }
    })
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
}
