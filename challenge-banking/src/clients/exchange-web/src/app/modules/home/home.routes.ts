import { Routes } from '@angular/router';
import { AuthGuard } from '@app/core/guards/auth.guard';
import { HomeComponent } from './pages/home/home.component';

export const HomeRoutes: Routes = [{ path: '', component: HomeComponent }];
