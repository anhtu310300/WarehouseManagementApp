import { ISpaceResponse } from '../interfaces/space.model';

export class SpaceModel implements ISpaceResponse {
  spaceId = 0;
  warehouseId = 0;
  spaceName = '';
  areaSqft = 0;
  description: string | null = null;
  isActive = true;
}
