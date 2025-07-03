import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { BatchDeleteComponent } from "./batch-delete/batch-delete.component";
import { UplaodT1safetyComponent } from "./uplaod-t1safety/uplaod-t1safety.component";
import { UploadT1safetyAddComponent } from "./upload-t1safety-add/upload-t1safety-add.component";

const routes: Routes = [
  {
    path: '',
    component: UplaodT1safetyComponent,
  },
  {
    path: "add",
    component: UploadT1safetyAddComponent,
    data: {
      title: "Add T1 Safety",
    },
  },
  {
    path: 'batch-delete',
    component: BatchDeleteComponent,
    data : {
      title : "Batch Delete"
    }
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class UploadT1safetyRoutingModule {}
