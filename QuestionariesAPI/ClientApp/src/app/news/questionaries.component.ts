import { Component, HostListener, Inject } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { ReCaptchaV3Service } from 'ng-recaptcha';
import { Observable } from 'rxjs';
import { Question } from '../_models/Question';
import { QuestionAnswer } from '../_models/QuestionAnswer';
import { AuthenticationService } from '../_services';
import { NewsApiService } from './questionaries-api.service';


@Component({
  selector: 'app-root',
  templateUrl: './questionaries.component.html',
  styleUrls: ['./questionaries.component.css']
})
export class NewsComponent {

  // declare empty arrays for articles and news sources
  isFetchingInProgress: boolean;
  questionareId: String;
  questionareAuthorized: boolean;
  mQuestions: Array<Question>;
  mPinCode: string;
  scoreVisible: boolean;
  TestScore: any;
  constructor(private route: ActivatedRoute, private newsapi: NewsApiService, public auth: AuthenticationService, private router: Router,public dialog: MatDialog,private recaptchaV3Service: ReCaptchaV3Service) {
    console.log('news component constructor called');
    this.mQuestions = [];
  }


  ngOnInit() {
    this.route.params.subscribe((params: Params) => {
      this.questionareId = params.id; 
      const element = {pin: ''};
      this.openPinDialog(element);
    });
  }

  // function to search for articles based on a news source (selected from UI mat-menu)

  openPinDialog(element: any): void {
    const dialogRef = this.dialog.open(AddArticleDialog, {
      width: '500px',
      data: {pin: element.pin},
    });

    dialogRef.afterClosed().subscribe(result => {
      console.log('The dialog was closed');
      if (result.pin.length != 4)
      {
        alert("Pin is invalid!");
        return this.openPinDialog(result);
      }
         
      this.recaptchaV3Service.execute('importantAction')
      .subscribe((token) => {
        var pinResult  =  {pinCode: result.pin, captchaChallenge: token};
        this.newsapi.initQuestionare(this.questionareId, pinResult).subscribe(x=> {
          if (x["type"] == 'questions'){
            this.mPinCode = pinResult.pinCode;
            this.questionareAuthorized = true;
            x['questions'].forEach(element => {
              this.scoreVisible = false;
              let question = {} as Question;
              question.answer = "0";
              question.questionId = Number.parseInt(element.questionId);
              question.questionText = element.questionText;
              this.mQuestions.push(question)})
            }
            if (x["type"] == 'result'){
               this.mPinCode = pinResult.pinCode;
               this.questionareAuthorized = true;
               this.scoreVisible = true;
               this.TestScore = x["questionare_result"]["questionarePoints"];
            }
          }
        );
      });
      
    });
  }

  public executeImportantAction(): void {
    
  }

  sendQuestionare(){
    var answers = Array<QuestionAnswer>();
    this.mQuestions.forEach(element => {
      let answer = {} as QuestionAnswer;
      answer.answer = Number.parseInt(element.answer);
      answer.questionId = element.questionId;
      answer.questionareId = this.questionareId;
      answers.push(answer);
    });
    var answerBody = {questionarePinCode: this.mPinCode, results: answers}
    this.newsapi.submitQuestionare(this.questionareId, answerBody).subscribe(x=>{
      if (x["type"] == 'result'){
        this.scoreVisible = true;
        this.TestScore = x["questionare_result"]["questionarePoints"];
      }
    })
  }

  // Putting delay because of fast api request (to show loading view only)
  delay(ms: number) {
    return new Promise( resolve => setTimeout(resolve, ms) );
  }
}

@Component({
  selector: 'pin.dialog',
  templateUrl: '/dialogs/pin.dialog.html',
})
export class AddArticleDialog {

    constructor(
      public dialogRef: MatDialogRef<AddArticleDialog>,
      @Inject(MAT_DIALOG_DATA) public data: any) {
      }
  
    onNoClick(): void {
      this.dialogRef.close();
    }
}
