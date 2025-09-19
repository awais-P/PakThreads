import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { TimelineRoutingModule } from './timeline-routing.module';
import { HomePageComponent } from './home-page/home-page.component';
import { CreatePostComponent } from './create-post/create-post.component';
import { HttpClientModule } from '@angular/common/http';
import { HighlightPipe } from '../../shared/pipes/highlight.pipe';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { FilterPipe } from '../../shared/pipes/filter.pipe';


@NgModule({
  declarations: [
    HomePageComponent,
    CreatePostComponent,
    HighlightPipe,
    FilterPipe
  ],
  imports: [
    CommonModule,
    TimelineRoutingModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule
  ]
})
export class TimelineModule { }
