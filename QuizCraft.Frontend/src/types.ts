export interface AnswerDto {
    text: string;
}

export interface QuestionDto {
    text: string;
    answers: AnswerDto[];
}

export interface QuizDto {
    id: string;
    createdAt: Date;
    questions: QuestionDto[];
}