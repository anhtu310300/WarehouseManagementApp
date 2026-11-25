import { ComponentFixture, TestBed } from '@angular/core/testing';
import { Subject } from 'rxjs';
import { WarehouseModel } from '../../../core/models/classes/warehouse.model';
import { SpaceModel } from '../../../core/models/classes/space.model';
import { IApiResponse } from '../../../core/models/interfaces/api-response.model';
import { SpaceModal } from './space-modal';

describe('SpaceModal', () => {
  let component: SpaceModal;
  let fixture: ComponentFixture<SpaceModal>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SpaceModal],
    }).compileComponents();

    fixture = TestBed.createComponent(SpaceModal);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('loads only spaces for the selected warehouse', async () => {
    const response = new Subject<IApiResponse<SpaceModel[]>>();
    const getByWarehouseId = vi.spyOn(component.spaceService, 'getByWarehouseId')
      .mockReturnValue(response.asObservable());

    component.selectedWarehouse = Object.assign(new WarehouseModel(), {
      warehouseId: 1,
      title: 'Test warehouse',
    });
    component.isOpen = true;
    component.getAllSpace();
    fixture.detectChanges();

    expect(getByWarehouseId).toHaveBeenCalledWith(1);
    expect(fixture.nativeElement.textContent).toContain('Loading spaces');

    response.next({
      data: [Object.assign(new SpaceModel(), {
        spaceId: 7,
        warehouseId: 1,
        spaceName: 'New space',
        areaSqft: 100,
        description: 'Test space',
        isActive: true,
      })],
      message: '',
      errorMessages: [],
      isSuccess: true,
    });
    fixture.detectChanges();

    expect(fixture.nativeElement.textContent).toContain('New space');
    expect(fixture.nativeElement.textContent).not.toContain('Loading spaces');
  });
});