import { inject } from "@angular/core";
import {ActivatedRoute, Router} from "@angular/router";
import { DestroyService } from "@services/destroy.service";
import { NgSnotifyService } from '@services/ng-snotify.service';
import { FunctionUtility } from "./function.utility";
import { NgxSpinnerService } from "ngx-spinner";
import { CommonService } from "@services/common/common.service";
import { TranslateService } from "@ngx-translate/core";
import { IconButton } from "@constants/common.constants";
export abstract class InjectBase {
    protected router = inject(Router);
    protected route = inject(ActivatedRoute);
    protected spinnerService = inject(NgxSpinnerService);
    protected snotifyService = inject(NgSnotifyService);
    protected destroyService = inject(DestroyService);
    protected functionUtility = inject(FunctionUtility);
    protected commonService = inject(CommonService);
    protected translateService = inject(TranslateService);
    protected iconButton = IconButton;
}
