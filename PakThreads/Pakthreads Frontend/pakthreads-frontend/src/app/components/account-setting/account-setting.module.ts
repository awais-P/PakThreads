import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AccountSettingRoutingModule } from './account-setting-routing.module';
import { AboutComponent } from './about/about.component';
import { ProfileComponent } from './profile/profile.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';


@NgModule({
  declarations: [
    AboutComponent,
    ProfileComponent
  ],
  imports: [
    CommonModule,
    AccountSettingRoutingModule,
    FormsModule,
    ReactiveFormsModule
    
  ]
})
export class AccountSettingModule { }
