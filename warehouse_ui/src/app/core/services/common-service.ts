import { Service } from '@angular/core';
import { Subject } from 'rxjs';

@Service()
export class CommonService {
    isShowLoader$ = new Subject<boolean>();
}
