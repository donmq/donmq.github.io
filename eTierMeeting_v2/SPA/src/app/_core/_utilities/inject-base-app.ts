import { inject } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
import { NgxSpinnerService } from "ngx-spinner";
import { NgSnotifyService } from "../_services/ng-snotify.service";
import { FunctionUtility } from "./function-utility";
import { DestroyService } from "../_services/destroy.service";
export abstract class InjectBase {
  protected router = inject(Router);
  protected route = inject(ActivatedRoute);
  protected spinnerService = inject(NgxSpinnerService);
  protected snotifyService = inject(NgSnotifyService);
  protected destroyService = inject(DestroyService);
  protected functionUtility = inject(FunctionUtility);
}
