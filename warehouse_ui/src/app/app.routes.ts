import { Routes } from '@angular/router';
import { Login } from './pages/auth/login/login';
import { Layout } from './pages/auth/layout/layout';
import { ClientList } from './pages/clients/client-list/client-list';
import { ClientForm } from './pages/clients/client-form/client-form';
import { authGuard } from './core/guards/auth-guard';
import { WarehouseMaster } from './pages/warehouse-master/warehouse-master';
import { EmployeeList } from './pages/employee/employee-list/employee-list';
import { NewEmployee } from './pages/employee/new-employee/new-employee';

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
      }
    ],
  },
];
