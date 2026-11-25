export interface ICreateSpaceRequest {
  warehouseId: number;
  spaceName: string;
  areaSqft: number;
  description: string | null;
}

export interface IUpdateSpaceRequest extends ICreateSpaceRequest {
  isActive: boolean;
}

export interface ISpaceResponse extends IUpdateSpaceRequest {
  spaceId: number;
}

export interface ISpaceAvailabilityResponse {
  spaceId: number;
  warehouseId: number;
  spaceName: string;
  totalAreaSqft: number;
  bookedAreaSqft: number;
  availableAreaSqft: number;
  isAvailable: boolean;
}