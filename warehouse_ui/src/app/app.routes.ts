import { Routes } from '@angular/router';
import { Login } from './pages/auth/login/login';
import { Layout } from './pages/auth/layout/layout';
import { ClientList } from './pages/clients/client-list/client-list';
import { ClientForm } from './pages/clients/client-form/client-form';
import { authGuard } from './core/guards/auth-guard';
import { WarehouseMaster } from './pages/warehouse-master/warehouse-master';
import { EmployeeList } from './pages/employee/employee-list/employee-list';
import { NewEmployee } from './pages/employee/new-employee/new-employee';
import { BookingList } from './pages/booking/booking-list/booking-list';
import { BookingForm } from './pages/booking/booking-form/booking-form';

export const routes: Routes = [
  {
    path: '',
    redirectTo: 'login',
    pathMatch: 'full',
  },
  {
    path: 'login',
    component: Login,
  },
  {
    path: 'admin',
    component: Layout,
    canActivate: [authGuard],
    children: [
      {
        path: 'client-list',
        component: ClientList,
      },
      {
        path: 'client-form/:id',
        component: ClientForm,
      },
      {
        path: 'warehouse-list',
        component: WarehouseMaster,
      },
      {
        path: 'employees',
        component: EmployeeList,
      },
      {
        path: 'employee-form/:empid',
        component: NewEmployee,
      },
      {
        path: 'bookings',
        component: BookingList,
      },
      {
        path: 'booking-form/:bookingid',
        component: BookingForm,
      },
    ],
  },
];
