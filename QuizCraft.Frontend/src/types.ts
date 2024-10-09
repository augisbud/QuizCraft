export interface AnswerDto {
    text: string;
}

export interface QuestionDto {
    text: string;
    answers: AnswerDto[];
}