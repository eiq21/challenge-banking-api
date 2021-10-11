import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { WelcomeRoutingModule } from './welcome-routing.module';
import { OnboardingComponent } from './pages/onboarding/onboarding.component';

@NgModule({
  declarations: [OnboardingComponent],
  imports: [CommonModule, WelcomeRoutingModule],
})
export class WelcomeModule {}
