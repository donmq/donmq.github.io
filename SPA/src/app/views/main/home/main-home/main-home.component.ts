import { Component, OnInit, TemplateRef, ViewChild, ViewContainerRef } from '@angular/core';
import { KeyValuePair } from '@utilities/key-value-pair';
import { Quality, DataCreate, MainHomeDto, MainHomeParam } from '@models/home';
import { HomeMainService } from './../../../../_core/services/home-main.service';
import { BsModalRef, BsModalService, ModalOptions } from 'ngx-bootstrap/modal';


@Component({
  selector: 'app-main-home',
  templateUrl: './main-home.component.html',
  styleUrl: './main-home.component.scss'
})
export class MainHomeComponent implements OnInit {
  data: MainHomeDto = <MainHomeDto>{};
  dataTable: Quality = <Quality>{};
  dataKetQua: Quality = <Quality>{};
  players: KeyValuePair[] = []
  exercise: KeyValuePair[] = []
  attribute: string[] = []
  attributeDisable: KeyValuePair[] = []
  stand: KeyValuePair[] = []
  param: MainHomeParam = <MainHomeParam>{};

  keys: KeyValuePair[] = [];
  dataAfter: Quality[] = []

  diemCongSang: number
  diemCongToi: number

  constructor(private modalService: BsModalService, private service: HomeMainService) { }

  ngOnInit() {
    this.getKeys();
    this.getListPlayer();
    this.getListExersice();
  }

  @ViewChild('templateTuChat', { read: TemplateRef }) templateTuChat!: TemplateRef<any>;
  @ViewChild('templateKetQua', { read: TemplateRef }) templateKetQua!: TemplateRef<any>;
  @ViewChild('templateSoSanh', { read: TemplateRef }) templateSoSanh!: TemplateRef<any>;
  modalRef?: BsModalRef;

  openTemplate(template: string) {
    const initialState: ModalOptions = {
      class: 'modal-md'
    };
    if (template == 'tuChat')
      this.modalRef = this.modalService.show(this.templateTuChat);
    else if (template == 'ketQua')
      this.modalRef = this.modalService.show(this.templateKetQua, initialState);
    else {
      this.getListCompares(this.param.inforID);
      this.modalRef = this.modalService.show(this.templateSoSanh);
    }

  }
  closeTemplate() {
    if (this.modalRef) {
      this.modalRef.hide();  // Đóng modal nếu đã mở
    }
  }

  onSelect() {
    if (this.param.inforID != null)
      this.getData();
  }

  getData() {
    this.service.getData(this.param).subscribe({
      next: res => {
        this.data = res
        this.dataAfter = this.data.qualityAfter
        this.chuyenThongTin();
        for (let index in this.dataAfter) {
          this.dataAfter[index].chatLuongChung = Math.floor(this.keys.reduce((a, b) => a + this.dataAfter[index][b.key], 0) / 15);
        }
      }
    })
  }

  //#region Tính toán
  tinhToan(index: number) {

    this.diemCong(index)
    this.keys.forEach(x => {
      if (index === 0) {
        this.dataAfter[index][x.key] = this.dataTable[x.key] + (this.attribute.includes(x.key) ? (this.attributeDisable.find(y => y.key === x.key)?.value === "1" ? this.diemCongSang : this.diemCongToi) : 0);

      } else {
        this.dataAfter[index][x.key] = this.dataKetQua[x.key] + (this.attribute.includes(x.key) ? (this.attributeDisable.find(y => y.key === x.key)?.value === "1" ? this.diemCongSang : this.diemCongToi) : 0);
      }
      this.dataKetQua[x.key] = this.dataAfter[index][x.key];
    });

    this.dataAfter[index].chatLuongChung = Math.floor(this.keys.reduce((a, b) => a + this.dataAfter[index][b.key], 0) / 15);
    this.dataKetQua.chatLuongChung = Math.floor(this.keys.reduce((a, b) => a + this.dataKetQua[b.key], 0) / 15);

  }

  //#endregion
  //#region Chuyển Thông tin
  chuyenThongTin() {

    this.dataTable.name = this.data.name
    this.dataTable.position = this.data.position

    this.keys.forEach(x => {
      this.dataTable[x.key] = +this.data[x.key];
    });
    this.dataTable.chatLuongChung = Math.floor(this.keys.reduce((a, b) => a + parseInt(this.data[b.key]), 0) / 15);

    this.keys.forEach(x => {
      this.dataKetQua[x.key] = this.dataAfter.length != 0 ? Math.max(...this.dataAfter.map(y => y[x.key])) : this.dataTable[x.key];
    });
    this.dataKetQua.chatLuongChung = Math.floor(this.keys.reduce((a, b) => a + parseInt(this.dataKetQua[b.key]), 0) / 15);

    this.dataAfter.forEach((x, index) => {
      this.getListThuocTinh(index, x, false)
    });

    this.getListDisable();
  }
  //#endregion
  diemTB: number = 180;
  addItem() {
    this.dataAfter.push(<Quality>{
      average: this.diemTB,
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

  dataCreate: DataCreate = <DataCreate>{
    dataTable: {},
    dataAfter: []
  };
  //region Save
  save(type: string) {
    this.dataTable.inforID = this.param.inforID
    this.dataCreate.dataTable = this.dataTable
    this.dataCreate.dataAfter = this.dataAfter
    if (type == 'add')
      this.service.create(this.dataCreate).subscribe({
        next: result => {
          if (result.isSuccess)
            this.getListPlayer();
        },
      })
    else {
      this.service.update(this.dataCreate).subscribe({
        next: result => {
          if (result.isSuccess) this.getListPlayer();
        }
      })
    }

  }
  //region Delete
  delete() {
    this.service.delete(this.param.inforID).subscribe({
      next: result => {
        if (result.isSuccess)
          this.getListPlayer();
      },
    })
  }

  removePlan(index: number){}

  removeItem(index: number) {
    // Kiểm tra chỉ số hợp lệ để xóa
    if (index >= 0 && index < this.dataAfter.length) {
      // Xóa phần tử tại index
      this.dataAfter.splice(index, 1); // Sử dụng 1 để chỉ xóa 1 phần tử

      console.log('Đã xóa phần tử tại index:', index);

      // Gọi lại hàm tinhToan cho tất cả các chỉ số sau index
      if (this.dataAfter[index] !== undefined) {
        this.changeBaiTap(index, this.dataAfter[index])
      }

      // Cập nhật lại kết quả dựa trên trạng thái mới của dataAfter
      if (index === 0) {
        this.keys.forEach(x => {
          this.dataKetQua[x.key] = this.dataTable[x.key];
        });
        this.dataKetQua.chatLuongChung = this.dataTable.chatLuongChung;
      } else {
        this.keys.forEach(x => {
          this.dataKetQua[x.key] = Math.max(...this.dataAfter.map(y => y[x.key]));
        });
        this.dataKetQua.chatLuongChung = Math.max(...this.dataAfter.map(x => x.chatLuongChung));
      }
    } else {
      console.log('Chỉ số không hợp lệ:', index);
    }
  }
  //region changeBaiTap
  changeBaiTap(index: number, baiTap: Quality) {
    this.clear(index);
    if (index === 0) {
      this.keys.forEach(x => {
        this.dataKetQua[x.key] = this.dataTable[x.key];
      });
      this.dataKetQua.chatLuongChung = this.dataTable.chatLuongChung;
    } else {
      this.keys.forEach(x => {
        this.dataKetQua[x.key] = Math.max(...this.dataAfter.map(y => y[x.key]));
      });
      this.dataKetQua.chatLuongChung = Math.max(...this.dataAfter.map(x => x.chatLuongChung));
    }
    this.getListThuocTinh(index, baiTap, true);

    // Kiểm tra nếu this.dataAfter[nextIndex] ko rỗng thì tiếp tục tính toán bằng đệ quy
    var nextIndex = index + 1
    if (this.dataAfter[nextIndex] !== undefined) {
      this.changeBaiTap(nextIndex, this.dataAfter[nextIndex])
    }

  }
  //region GetListAttribute
  getListThuocTinh(index: number, exercise: Quality, tinhtoan: boolean) {
    this.service.getListThuocTinh(exercise.exerciseID, this.data.position).subscribe({
      next: res => {
        this.attributeDisable = res
        this.attribute = this.attributeDisable.map(x => x.key)

        if (tinhtoan == true)
          this.tinhToan(index);

        this.styleBaiTapBasedOnAttribute(index);
      }
    })
  }

  // Hiển thị các giá trị được cộng sau hàm tính toán
  styleBaiTapBasedOnAttribute(index: number) {
    // Lọc ra các bài tập có trong attribute và áp dụng style
    let baiTapElements = document.querySelectorAll(`.red-border-${index}`);

    // Lấy thẻ div trong thẻ td
    baiTapElements.forEach((element: HTMLElement) => {
      let divElement = element.querySelector('div');

      if (this.attribute.includes(divElement.className)) {
        // Áp dụng style border đỏ cho các bài tập có trong attribute
        divElement.style.border = '0.1em solid red'
        divElement.style.visibility = 'visible';

      } else {
        // Ẩn giá trị
        divElement.style.visibility = 'hidden';
      }
    });
  }

  // region GetList
  getListDisable() {
    this.service.getListDisable(this.data.position).subscribe({
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
  keyPhongThu
  keyTanCong
  keyTheChat
  trungBinh = "Trung bình"
  getKeys() {
    this.service.getKeys().subscribe({
      next: res => {
        this.keys = res
        this.keyPhongThu = res.filter(x => x.value == '1')
        this.keyTanCong = res.filter(x => x.value == '2')
        this.keyTheChat = res.filter(x => x.value == '3')

      }
    })
  }

  clear(index: number) {
    this.keys.forEach(x => {
      this.dataAfter[index][x.key] = null
    });
  }

  ketQua(index: number) {
    this.keys.forEach(x => {
      this.dataKetQua[x.key] = this.data[x.key]
    });
  }

  listCompares: Quality[]
  getListCompares(inforID: number) {
    this.service.getListCompares(inforID).subscribe({
      next: res => {
        this.listCompares = res.map(item => ({
          ...item,
          canDelete: true // Thêm thuộc tính canDelete cho từng đối tượng
        }));
      }
    })
  }

  compare: Quality
  createCompare() {
    this.compare.inforID = this.param.inforID
    this.compare.chatLuongChung = this.dataKetQua.chatLuongChung
    this.compare.canPha = this.dataKetQua.canPha
    this.compare.kemNguoi = this.dataKetQua.kemNguoi
    this.compare.chayCho = this.dataKetQua.chayCho
    this.compare.danhDau = this.dataKetQua.danhDau
    this.compare.dungCam = this.dataKetQua.dungCam
    this.compare.chuyenBong = this.dataKetQua.chuyenBong
    this.compare.reBong = this.dataKetQua.reBong
    this.compare.tatCanh = this.dataKetQua.tatCanh
    this.compare.sutManh = this.dataKetQua.sutManh
    this.compare.dutDiem = this.dataKetQua.dutDiem
    this.compare.theLuc = this.dataKetQua.theLuc
    this.compare.sucManh = this.dataKetQua.sucManh
    this.compare.xongXao = this.dataKetQua.xongXao
    this.compare.tocDo = this.dataKetQua.tocDo
    this.compare.sangTao = this.dataKetQua.sangTao
    this.service.createCompare(this.compare).subscribe({
      next: res => {
        this.getListCompares(this.compare.inforID);
      }
    })
  }

  deleteCompare(data: Quality) {
    this.service.deleteCompare(data).subscribe({
      next: res => {
        this.getListCompares(data.inforID);
      }
    })
  }
  //#region DiemCong
  diemCong(index: number) {
    let totalSum = 0;

    let lowercaseAttribute = this.attribute.map(a => a.toLowerCase());

    Object.keys(this.dataKetQua).forEach(key => {
      let lowercaseKey = key.toLowerCase();
      if (lowercaseAttribute.includes(lowercaseKey)) {
        totalSum += index == 0 ? this.dataTable[key] : this.dataKetQua[key];
      }
    });
    let soDiemCong: number
    if (index == 0)
      soDiemCong = this.attribute.length * this.dataAfter[index].average - totalSum
    else
      soDiemCong = this.attribute.length * this.dataAfter[index].average - totalSum
    this.dataAfter[index].soDiemTap = soDiemCong

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
    if (countValueOne == 0)
      tongDiemToi = soDiemCong
    let tongDiemSang = soDiemCong - tongDiemToi;

    if (this.attributeDisable.length == 5) {
      if (countValueOne === 5) {
        this.diemCongSang = Math.ceil(tongDiemSang / 5);
        this.diemCongToi = 0
      } else if (countValueOne === 4) {
        this.diemCongSang = Math.ceil(tongDiemSang / 4);
        this.diemCongToi = Math.ceil(tongDiemToi);
      } else if (countValueOne === 3) {
        this.diemCongSang = Math.ceil(tongDiemSang / 3);
        this.diemCongToi = Math.ceil(tongDiemToi / 2);
      } else if (countValueOne === 2) {
        this.diemCongSang = Math.ceil(tongDiemSang / 2);
        this.diemCongToi = Math.ceil(tongDiemToi / 3);
      } else if (countValueOne === 1) {
        this.diemCongSang = Math.ceil(tongDiemSang);
        this.diemCongToi = Math.ceil(tongDiemToi / 4);
      } else if (countValueOne === 0) {
        this.diemCongSang = 0;
        this.diemCongToi = Math.ceil(tongDiemToi / 5);
      }
    }
    if (this.attributeDisable.length == 4) {
      if (countValueOne === 4) {
        this.diemCongSang = Math.ceil(tongDiemSang / 4);
        this.diemCongToi = 0
      } else if (countValueOne === 3) {
        this.diemCongSang = Math.ceil(tongDiemSang / 3);
        this.diemCongToi = Math.ceil(tongDiemToi);
      } else if (countValueOne === 2) {
        this.diemCongSang = Math.ceil(tongDiemSang / 2);
        this.diemCongToi = Math.ceil(tongDiemToi / 2);
      } else if (countValueOne === 1) {
        this.diemCongSang = Math.ceil(tongDiemSang);
        this.diemCongToi = Math.ceil(tongDiemToi / 3);
      } else if (countValueOne === 0) {
        this.diemCongSang = 0;
        this.diemCongToi = Math.ceil(tongDiemToi / 4);
      }
    }
    if (this.attributeDisable.length == 3) {
      if (countValueOne === 3) {
        this.diemCongSang = Math.ceil(tongDiemSang / 3);
        this.diemCongToi = 0
      } else if (countValueOne === 2) {
        this.diemCongSang = Math.ceil(tongDiemSang / 2);
        this.diemCongToi = Math.ceil(tongDiemToi);
      } else if (countValueOne === 1) {
        this.diemCongSang = Math.ceil(tongDiemSang);
        this.diemCongToi = Math.ceil(tongDiemToi / 2);
      } else if (countValueOne === 0) {
        this.diemCongSang = 0;
        this.diemCongToi = Math.ceil(tongDiemToi / 3);
      }
    }
    if (this.attributeDisable.length == 2) {
      if (countValueOne === 2) {
        this.diemCongSang = Math.ceil(tongDiemSang / 2);
        this.diemCongToi = 0
      } else if (countValueOne === 1) {
        this.diemCongSang = Math.ceil(tongDiemSang);
        this.diemCongToi = Math.ceil(tongDiemToi);
      } else if (countValueOne === 0) {
        this.diemCongSang = 0;
        this.diemCongToi = Math.ceil(tongDiemToi / 2);
      }
    }
  }
  // #endregion
}
