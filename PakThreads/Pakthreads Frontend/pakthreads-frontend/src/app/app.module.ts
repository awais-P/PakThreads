import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppComponent } from './app.component';
import { AuthModule } from './components/auth/auth.module';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { authInterceptor } from './shared/interceptors/auth.interceptor';
import { TimelineModule } from './components/timeline/timeline.module';
import { NavbarComponent } from './shared/components/navbar/navbar.component';
import { AppRoutingModule } from './app-routing.module';
import { AccountSettingModule } from './components/account-setting/account-setting.module';
import { FormsModule } from '@angular/forms';
import { FilterPipe } from './shared/pipes/filter.pipe';

@NgModule({
  declarations: [
    AppComponent,
    NavbarComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    AuthModule,
    TimelineModule,
    AccountSettingModule,
    FormsModule
  ],
  providers: [
    {
    provide: HTTP_INTERCEPTORS,
    useClass: authInterceptor,
    multi: true
  }
    ],
  bootstrap: [AppComponent]
})
export class AppModule { }
