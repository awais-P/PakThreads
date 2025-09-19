import { Component, inject, OnInit } from '@angular/core';
import { PayloadService } from '../../../shared/services/payload.service';
import { ApiCallService } from '../../../shared/services/api-call.service';
import { ActivatedRoute } from '@angular/router';


@Component({
  selector: 'app-home-page',
  templateUrl: './home-page.component.html',
  styleUrls: ['./home-page.component.css']
})




export class HomePageComponent implements OnInit {

  defaultImg = 'assets/images/no-image-found.jpg';
  posts: any[] = [];
  public _apiCall = inject(ApiCallService);
  private readonly route  = inject(ActivatedRoute);
  searchText = '';
  noResults = false;
  showCreatePost = false;
  customTag: string = '';
  filterApplied: boolean = false;
  

  tagSearch: string = '';
  majorTags: string[] = ['Gaming', 'Science', 'Technology', 'News', 'Sports', 'Music', 'Movies','Food'];
  selectedTag: string | null = null;
  


  ngOnInit() {
  this.route.queryParams.subscribe(params => {
    const search = params['search'] || '';
    this.searchText = search;
    this.getPosts(this.searchText);
  });
}

selectTag(tag: string): void {
  this.selectedTag = tag;
}

applyFilter(): void {
  const finalTag = this.customTag.trim() || this.selectedTag;
  if (!finalTag) return;

  this.filterApplied = true;
  this.getPosts('', finalTag);
}

clearFilter(): void {
  this.selectedTag = '';
  this.customTag = '';
  this.filterApplied = false;
  this.getPosts(); // Load all posts
}

filterPostsByTag(): void {
  if (this.selectedTag) {
    this.getPosts('', this.selectedTag);
  }
}

  getPosts(searchText: string = '', filter: string = ''): void {
  const payload = {
    searchText: searchText,
    getName: '',
    filter: filter
  };

    this._apiCall.PostWithoutToken(payload, 'Post/GetPostData').subscribe(res => {
      if (res.responseCode === 200) {
        this.posts = res.data;
        this.noResults = this.posts.length === 0;
      }
    });
  }

  onImgError(event: Event): void {
    (event.target as HTMLImageElement).src = this.defaultImg;
  }


  timeSince(dateString: string): string {
  // Convert to ISO format: "2025-06-17T10:14:39.3664957"
  const isoDate = dateString.replace(' ', 'T');

  const now = new Date();
  const then = new Date(isoDate);

  if (isNaN(then.getTime())) return 'Invalid date';

  const seconds = Math.floor((now.getTime() - then.getTime()) / 1000);

  const intervalMap = [
    { label: 'year', seconds: 31536000 },
    { label: 'month', seconds: 2592000 },
    { label: 'week', seconds: 604800 },
    { label: 'day', seconds: 86400 },
    { label: 'hour', seconds: 3600 },
    { label: 'minute', seconds: 60 },
  ];

  for (const interval of intervalMap) {
    const count = Math.floor(seconds / interval.seconds);
    if (count > 0) {
      return `${count} ${interval.label}${count > 1 ? 's' : ''} ago`;
    }
  }
  
  return 'Just now';
}
  

vote(post: any, type: 'up' | 'down') {
  const previousVote = post.userVote;

  if (type === 'up') {
    if (previousVote === 'up') {
      post.upvotes--;
      post.userVote = null;
    } else {
      if (previousVote === 'down') post.downvotes--;
      post.upvotes++;
      if (previousVote !== 'up') post.userVote = 'up';
    }
  }

  if (type === 'down') {
    if (previousVote === 'down') {
      post.downvotes--;
      post.userVote = null;
    } else {
      if (previousVote === 'up') post.upvotes--;
      post.downvotes++;
      if (previousVote !== 'down') post.userVote = 'down';
    }
  }

  // Optional: Call backend to persist
  // this._apiCall.PostWithToken({ postId: post.id, vote: post.userVote }, "Post/Vote").subscribe();
}



copyShareLink(postId: number) {
  const url = `${window.location.origin}/post/${postId}`;
  navigator.clipboard.writeText(url)
    .then(() => alert('Post link copied!'))
    .catch(() => alert('Failed to copy link'));
}



highlight(text: string): string {
  if (!this.searchText) return text;
  const escaped = this.searchText.replace(/[.*+?^${}()|[\]\\]/g, '\\$&'); // escape regex
  const regex = new RegExp(`(${escaped})`, 'gi');
  return text.replace(regex, '<mark>$1</mark>');
}


viewingUserPosts = false;

loadUserPosts(searchText: string = '', filter: string = ''): void {
  const payload = {
    searchText: searchText,
    getName: '',
    filter: filter
  };

  this.viewingUserPosts = true;
  this.filterApplied = true;

  this._apiCall
    .PostWithToken(payload , "Post/GetUserPosts") 
    .subscribe({
      next: (res: any) => {
        if (res.responseCode === 200) {
          this.posts = res.data || [];
          this.noResults = this.posts.length === 0;
        } else {
          this.posts = [];
          this.noResults = true;
        }
      },
      error: (err) => {
        console.error('Failed to load user posts:', err);
        this.posts = [];
        this.noResults = true;
      }
    });
}



}


