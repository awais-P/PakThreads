import { Component, EventEmitter, inject, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ApiCallService } from '../../../shared/services/api-call.service';

@Component({
  selector: 'app-create-post',
  templateUrl: './create-post.component.html',
  styleUrl: './create-post.component.css'
})
export class CreatePostComponent {
postForm!: FormGroup;
  tagInput: string = '';
  postTags: string[] = [];
  selectedFileName: string = '';

  public _apiCall = inject(ApiCallService);

  @Output() close = new EventEmitter<void>();

  closeModal(): void {
    this.close.emit();
  }

  constructor(private fb: FormBuilder) {}

  ngOnInit(): void {
    this.postForm = this.fb.group({
      title: ['', Validators.required],
      postType: ['', Validators.required],
      contentUrl: ['']
    });
  }

  addTag(event: Event): void {
  event.preventDefault();
  const input = (event.target as HTMLInputElement);
  const trimmed = this.tagInput.trim();

  if (
    trimmed &&
    this.postTags.length < 5 &&
    !this.postTags.includes(trimmed)
  ) {
    this.postTags.push(trimmed);
  }

  this.tagInput = '';
}



  removeTag(index: number): void {
    this.postTags.splice(index, 1);
  }

  handleFileUpload(event: Event): void {
  const file = (event.target as HTMLInputElement).files?.[0];
  if (!file) return;

  if (file.size > 2 * 1024 * 1024) {
    alert('File must be smaller than 2MB');
    return;
  }

  const reader = new FileReader();
  reader.onload = () => {
    this.postForm.patchValue({
      contentUrl: reader.result as string
    });
    this.selectedFileName = file.name;
  };
  reader.readAsDataURL(file);
}



  submitPost(): void {
  this.postForm.markAllAsTouched();

  const errorMessage = this.getPostValidationError();
  if (errorMessage) {
    return;
  }

  const payload = this.createPostPayload();
  console.log("Submitting Payload:", payload);

  this._apiCall
    .PostWithToken(payload, "Post/CreatePost")
    .subscribe({
      next: (response: any) => {
        if (response.responseCode === 200) {
          this.resetPostForm();
          this.closeModal(); 
        } else {
        }
      },
      error: (err) => {
        console.error("API error:", err);
      }
    });
}

          

private createPostPayload(): any {
  return {
    title: this.postForm.get("title")?.value,
    postType: this.postForm.get("postType")?.value,
    contentUrl: this.postForm.get("contentUrl")?.value,
    postTagsVms: this.postTags.map(tag => ({
      tagName: tag
    }))
  };
}


private getPostValidationError(): string | null {
  if (this.postForm.invalid) return "Please complete all required fields.";
  if (this.postTags.length < 1) return "Please add at least one tag.";
  return null;
}

private resetPostForm(): void {
  this.postForm.reset();
  this.postTags = [];
  this.tagInput = '';
}


}