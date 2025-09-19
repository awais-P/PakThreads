import { inject, Injectable } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import { ApiCallService } from './api-call.service';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class PayloadService {

  private _apiCall = inject(ApiCallService);

  constructor() { }



//  createInstitutePayload(
//     filters: any = {},
//     pageNumber: number = 1,
//     pageSize: number = 8
//   ): any {
//     return {
//       countryName: filters.country || "",
//       stateName: filters.state || "",
//       cityName: filters.city || "",
//       searchText: filters.search || "",
//       instituteType: filters.instituteType || "",
//       pageNumber: pageNumber,
//       pageSize: 8,
//       getName: "",
//     };
//   }


getAllPosts(filters: any = {}): Observable<any[]> {
  const payload = this.createPostPayload(filters);

  return this._apiCall
    .PostWithoutToken(payload, "Post/GetPostData")
    .pipe(
      map((response: any) => {
        return response.data.map((post: any) => ({
          id: post.id,
          title: post.title,
          userName: post.userName,
          profileImageUrl: {
            url: post.profileImageUrl,
            fileType: "image"
          },
          postTagsVms: post.postTagsVms?.map((tag: any) => ({
            id: tag.id,
            postId: tag.postId,
            tagName: tag.tagName
          })) || [],
          contentUrl: post.contentUrl,
          downvotes: post.downvotes,
          upvotes: post.upvotes,
          contactNumber: post.contactNumber,
          addedDate: post.addedDate,
        }));
      })
    );
}




createPostPayload(
    filters: any = {}
  ): any {
    return {
      searchText: filters.searchText || "",
      filter: filters.filter || "",
      getName: "",
    };
  }


}
