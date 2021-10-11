import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { WelcomeRoutes } from './welcome.routes';

@NgModule({
  imports: [RouterModule.forChild(WelcomeRoutes)],
  exports: [RouterModule],
})
export class WelcomeRoutingModule {}
