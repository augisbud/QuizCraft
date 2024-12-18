//----------------------
// <auto-generated>
//     Generated using the NSwag toolchain v14.1.0.0 (NJsonSchema v11.0.2.0 (Newtonsoft.Json v13.0.0.0)) (http://NSwag.org)
// </auto-generated>
//----------------------

/* tslint:disable */
/* eslint-disable */
// ReSharper disable InconsistentNaming

export class Client {
    private http: { fetch(url: RequestInfo, init?: RequestInit): Promise<Response> };
    private baseUrl: string;
    protected jsonParseReviver: ((key: string, value: any) => any) | undefined = undefined;

    constructor(baseUrl?: string, http?: { fetch(url: RequestInfo, init?: RequestInit): Promise<Response> }) {
        this.http = http ? http : window as any;
        this.baseUrl = baseUrl ?? "";
    }

    /**
     * @param contentType (optional) 
     * @param contentDisposition (optional) 
     * @param headers (optional) 
     * @param length (optional) 
     * @param name (optional) 
     * @param fileName (optional) 
     * @return Success
     */
    quizzesPOST(contentType: string | undefined, contentDisposition: string | undefined, headers: { [key: string]: string[]; } | undefined, length: number | undefined, name: string | undefined, fileName: string | undefined): Promise<string> {
        let url_ = this.baseUrl + "/quizzes";
        url_ = url_.replace(/[?&]$/, "");

        const content_ = new FormData();
        if (contentType === null || contentType === undefined)
            throw new Error("The parameter 'contentType' cannot be null.");
        else
            content_.append("ContentType", contentType.toString());
        if (contentDisposition === null || contentDisposition === undefined)
            throw new Error("The parameter 'contentDisposition' cannot be null.");
        else
            content_.append("ContentDisposition", contentDisposition.toString());
        if (headers === null || headers === undefined)
            throw new Error("The parameter 'headers' cannot be null.");
        else
            content_.append("Headers", JSON.stringify(headers));
        if (length === null || length === undefined)
            throw new Error("The parameter 'length' cannot be null.");
        else
            content_.append("Length", length.toString());
        if (name === null || name === undefined)
            throw new Error("The parameter 'name' cannot be null.");
        else
            content_.append("Name", name.toString());
        if (fileName === null || fileName === undefined)
            throw new Error("The parameter 'fileName' cannot be null.");
        else
            content_.append("FileName", fileName.toString());

        let options_: RequestInit = {
            body: content_,
            method: "POST",
            headers: {
                "Accept": "text/plain"
            }
        };

        return this.http.fetch(url_, options_).then((_response: Response) => {
            return this.processQuizzesPOST(_response);
        });
    }

    protected processQuizzesPOST(response: Response): Promise<string> {
        const status = response.status;
        let _headers: any = {}; if (response.headers && response.headers.forEach) { response.headers.forEach((v: any, k: any) => _headers[k] = v); };
        if (status === 200) {
            return response.text().then((_responseText) => {
                let result200: any = null;
                let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
                result200 = resultData200 !== undefined ? resultData200 : <any>null;

                return result200;
            });
        } else if (status !== 200 && status !== 204) {
            return response.text().then((_responseText) => {
                return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            });
        }
        return Promise.resolve<string>(null as any);
    }

    /**
     * @return Success
     */
    quizzesAll(): Promise<QuizDto[]> {
        let url_ = this.baseUrl + "/quizzes";
        url_ = url_.replace(/[?&]$/, "");

        let options_: RequestInit = {
            method: "GET",
            headers: {
                "Accept": "text/plain"
            }
        };

        return this.http.fetch(url_, options_).then((_response: Response) => {
            return this.processQuizzesAll(_response);
        });
    }

    protected processQuizzesAll(response: Response): Promise<QuizDto[]> {
        const status = response.status;
        let _headers: any = {}; if (response.headers && response.headers.forEach) { response.headers.forEach((v: any, k: any) => _headers[k] = v); };
        if (status === 200) {
            return response.text().then((_responseText) => {
                let result200: any = null;
                let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
                if (Array.isArray(resultData200)) {
                    result200 = [] as any;
                    for (let item of resultData200)
                        result200!.push(QuizDto.fromJS(item));
                }
                else {
                    result200 = <any>null;
                }
                return result200;
            });
        } else if (status !== 200 && status !== 204) {
            return response.text().then((_responseText) => {
                return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            });
        }
        return Promise.resolve<QuizDto[]>(null as any);
    }

    /**
     * @return Success
     */
    questionsGET(id: string): Promise<DetailedQuizDto> {
        let url_ = this.baseUrl + "/quizzes/{id}/questions";
        if (id === undefined || id === null)
            throw new Error("The parameter 'id' must be defined.");
        url_ = url_.replace("{id}", encodeURIComponent("" + id));
        url_ = url_.replace(/[?&]$/, "");

        let options_: RequestInit = {
            method: "GET",
            headers: {
                "Accept": "text/plain"
            }
        };

        return this.http.fetch(url_, options_).then((_response: Response) => {
            return this.processQuestionsGET(_response);
        });
    }

    protected processQuestionsGET(response: Response): Promise<DetailedQuizDto> {
        const status = response.status;
        let _headers: any = {}; if (response.headers && response.headers.forEach) { response.headers.forEach((v: any, k: any) => _headers[k] = v); };
        if (status === 200) {
            return response.text().then((_responseText) => {
                let result200: any = null;
                let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
                result200 = DetailedQuizDto.fromJS(resultData200);
                return result200;
            });
        } else if (status !== 200 && status !== 204) {
            return response.text().then((_responseText) => {
                return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            });
        }
        return Promise.resolve<DetailedQuizDto>(null as any);
    }

    /**
     * @param body (optional) 
     * @return Success
     */
    questionsPOST(quizId: string, questionId: string, body: AnswerAttemptDto | undefined): Promise<ValidatedAnswerDto> {
        let url_ = this.baseUrl + "/quizzes/{quizId}/questions/{questionId}";
        if (quizId === undefined || quizId === null)
            throw new Error("The parameter 'quizId' must be defined.");
        url_ = url_.replace("{quizId}", encodeURIComponent("" + quizId));
        if (questionId === undefined || questionId === null)
            throw new Error("The parameter 'questionId' must be defined.");
        url_ = url_.replace("{questionId}", encodeURIComponent("" + questionId));
        url_ = url_.replace(/[?&]$/, "");

        const content_ = JSON.stringify(body);

        let options_: RequestInit = {
            body: content_,
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                "Accept": "text/plain"
            }
        };

        return this.http.fetch(url_, options_).then((_response: Response) => {
            return this.processQuestionsPOST(_response);
        });
    }

    protected processQuestionsPOST(response: Response): Promise<ValidatedAnswerDto> {
        const status = response.status;
        let _headers: any = {}; if (response.headers && response.headers.forEach) { response.headers.forEach((v: any, k: any) => _headers[k] = v); };
        if (status === 200) {
            return response.text().then((_responseText) => {
                let result200: any = null;
                let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
                result200 = ValidatedAnswerDto.fromJS(resultData200);
                return result200;
            });
        } else if (status !== 200 && status !== 204) {
            return response.text().then((_responseText) => {
                return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            });
        }
        return Promise.resolve<ValidatedAnswerDto>(null as any);
    }

    /**
     * @return Success
     */
    quizzesPOST2(quizId: string): Promise<void> {
        let url_ = this.baseUrl + "/quizzes/{quizId}";
        if (quizId === undefined || quizId === null)
            throw new Error("The parameter 'quizId' must be defined.");
        url_ = url_.replace("{quizId}", encodeURIComponent("" + quizId));
        url_ = url_.replace(/[?&]$/, "");

        let options_: RequestInit = {
            method: "POST",
            headers: {
            }
        };

        return this.http.fetch(url_, options_).then((_response: Response) => {
            return this.processQuizzesPOST2(_response);
        });
    }

    protected processQuizzesPOST2(response: Response): Promise<void> {
        const status = response.status;
        let _headers: any = {}; if (response.headers && response.headers.forEach) { response.headers.forEach((v: any, k: any) => _headers[k] = v); };
        if (status === 200) {
            return response.text().then((_responseText) => {
                return;
            });
        } else if (status !== 200 && status !== 204) {
            return response.text().then((_responseText) => {
                return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            });
        }
        return Promise.resolve<void>(null as any);
    }

    /**
     * @return Success
     */
    quizzesDELETE(quizId: string): Promise<void> {
        let url_ = this.baseUrl + "/quizzes/{quizId}";
        if (quizId === undefined || quizId === null)
            throw new Error("The parameter 'quizId' must be defined.");
        url_ = url_.replace("{quizId}", encodeURIComponent("" + quizId));
        url_ = url_.replace(/[?&]$/, "");

        let options_: RequestInit = {
            method: "DELETE",
            headers: {
            }
        };

        return this.http.fetch(url_, options_).then((_response: Response) => {
            return this.processQuizzesDELETE(_response);
        });
    }

    protected processQuizzesDELETE(response: Response): Promise<void> {
        const status = response.status;
        let _headers: any = {}; if (response.headers && response.headers.forEach) { response.headers.forEach((v: any, k: any) => _headers[k] = v); };
        if (status === 200) {
            return response.text().then((_responseText) => {
                return;
            });
        } else if (status !== 200 && status !== 204) {
            return response.text().then((_responseText) => {
                return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            });
        }
        return Promise.resolve<void>(null as any);
    }

    /**
     * @return PDF file generated
     */
    exportPdf(quizId: string): Promise<FileResponse> {
        let url_ = this.baseUrl + "/{quizId}/export-pdf";
        if (quizId === undefined || quizId === null)
            throw new Error("The parameter 'quizId' must be defined.");
        url_ = url_.replace("{quizId}", encodeURIComponent("" + quizId));
        url_ = url_.replace(/[?&]$/, "");

        let options_: RequestInit = {
            method: "GET",
            headers: {
                "Accept": "application/pdf"
            }
        };

        return this.http.fetch(url_, options_).then((_response: Response) => {
            return this.processExportPdf(_response);
        });
    }

    protected processExportPdf(response: Response): Promise<FileResponse> {
        const status = response.status;
        let _headers: any = {}; if (response.headers && response.headers.forEach) { response.headers.forEach((v: any, k: any) => _headers[k] = v); };
        if (status === 200 || status === 206) {
            const contentDisposition = response.headers ? response.headers.get("content-disposition") : undefined;
            let fileNameMatch = contentDisposition ? /filename\*=(?:(\\?['"])(.*?)\1|(?:[^\s]+'.*?')?([^;\n]*))/g.exec(contentDisposition) : undefined;
            let fileName = fileNameMatch && fileNameMatch.length > 1 ? fileNameMatch[3] || fileNameMatch[2] : undefined;
            if (fileName) {
                fileName = decodeURIComponent(fileName);
            } else {
                fileNameMatch = contentDisposition ? /filename="?([^"]*?)"?(;|$)/g.exec(contentDisposition) : undefined;
                fileName = fileNameMatch && fileNameMatch.length > 1 ? fileNameMatch[1] : undefined;
            }
            return response.blob().then(blob => { return { fileName: fileName, data: blob, status: status, headers: _headers }; });
        } else if (status !== 200 && status !== 204) {
            return response.text().then((_responseText) => {
                return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            });
        }
        return Promise.resolve<FileResponse>(null as any);
    }

    /**
     * @return Success
     */
    quizzesGET(id: string): Promise<QuizAttemptsDto> {
        let url_ = this.baseUrl + "/statistics/individual/quizzes/{id}";
        if (id === undefined || id === null)
            throw new Error("The parameter 'id' must be defined.");
        url_ = url_.replace("{id}", encodeURIComponent("" + id));
        url_ = url_.replace(/[?&]$/, "");

        let options_: RequestInit = {
            method: "GET",
            headers: {
                "Accept": "text/plain"
            }
        };

        return this.http.fetch(url_, options_).then((_response: Response) => {
            return this.processQuizzesGET(_response);
        });
    }

    protected processQuizzesGET(response: Response): Promise<QuizAttemptsDto> {
        const status = response.status;
        let _headers: any = {}; if (response.headers && response.headers.forEach) { response.headers.forEach((v: any, k: any) => _headers[k] = v); };
        if (status === 200) {
            return response.text().then((_responseText) => {
                let result200: any = null;
                let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
                result200 = QuizAttemptsDto.fromJS(resultData200);
                return result200;
            });
        } else if (status !== 200 && status !== 204) {
            return response.text().then((_responseText) => {
                return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            });
        }
        return Promise.resolve<QuizAttemptsDto>(null as any);
    }

    /**
     * @return Success
     */
    global(): Promise<GlobalStatsDto> {
        let url_ = this.baseUrl + "/statistics/global";
        url_ = url_.replace(/[?&]$/, "");

        let options_: RequestInit = {
            method: "GET",
            headers: {
                "Accept": "text/plain"
            }
        };

        return this.http.fetch(url_, options_).then((_response: Response) => {
            return this.processGlobal(_response);
        });
    }

    protected processGlobal(response: Response): Promise<GlobalStatsDto> {
        const status = response.status;
        let _headers: any = {}; if (response.headers && response.headers.forEach) { response.headers.forEach((v: any, k: any) => _headers[k] = v); };
        if (status === 200) {
            return response.text().then((_responseText) => {
                let result200: any = null;
                let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
                result200 = GlobalStatsDto.fromJS(resultData200);
                return result200;
            });
        } else if (status !== 200 && status !== 204) {
            return response.text().then((_responseText) => {
                return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            });
        }
        return Promise.resolve<GlobalStatsDto>(null as any);
    }
}

export class AnswerAttemptDto implements IAnswerAttemptDto {
    answerId?: string;

    constructor(data?: IAnswerAttemptDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(_data?: any) {
        if (_data) {
            this.answerId = _data["answerId"];
        }
    }

    static fromJS(data: any): AnswerAttemptDto {
        data = typeof data === 'object' ? data : {};
        let result = new AnswerAttemptDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["answerId"] = this.answerId;
        return data;
    }
}

export interface IAnswerAttemptDto {
    answerId?: string;
}

export class AnswerDto implements IAnswerDto {
    id?: string;
    text?: string | undefined;

    constructor(data?: IAnswerDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(_data?: any) {
        if (_data) {
            this.id = _data["id"];
            this.text = _data["text"];
        }
    }

    static fromJS(data: any): AnswerDto {
        data = typeof data === 'object' ? data : {};
        let result = new AnswerDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["id"] = this.id;
        data["text"] = this.text;
        return data;
    }
}

export interface IAnswerDto {
    id?: string;
    text?: string | undefined;
}

export enum Category {
    GeneralKnowledge = "GeneralKnowledge",
    Science = "Science",
    History = "History",
    Geography = "Geography",
    Sports = "Sports",
    Entertainment = "Entertainment",
    Technology = "Technology",
    Mathematics = "Mathematics",
    Literature = "Literature",
    Music = "Music",
    Art = "Art",
    Movies = "Movies",
    Politics = "Politics",
    Language = "Language",
    Religion = "Religion",
    FoodAndDrink = "FoodAndDrink",
    Nature = "Nature",
    Health = "Health",
    Business = "Business",
    Travel = "Travel",
}

export class DetailedQuizDto implements IDetailedQuizDto {
    id!: string;
    title!: string;
    currentQuestionId!: string;
    questions!: QuestionDto[];

    constructor(data?: IDetailedQuizDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
        if (!data) {
            this.questions = [];
        }
    }

    init(_data?: any) {
        if (_data) {
            this.id = _data["id"];
            this.title = _data["title"];
            this.currentQuestionId = _data["currentQuestionId"];
            if (Array.isArray(_data["questions"])) {
                this.questions = [] as any;
                for (let item of _data["questions"])
                    this.questions!.push(QuestionDto.fromJS(item));
            }
        }
    }

    static fromJS(data: any): DetailedQuizDto {
        data = typeof data === 'object' ? data : {};
        let result = new DetailedQuizDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["id"] = this.id;
        data["title"] = this.title;
        data["currentQuestionId"] = this.currentQuestionId;
        if (Array.isArray(this.questions)) {
            data["questions"] = [];
            for (let item of this.questions)
                data["questions"].push(item.toJSON());
        }
        return data;
    }
}

export interface IDetailedQuizDto {
    id: string;
    title: string;
    currentQuestionId: string;
    questions: QuestionDto[];
}

export class GlobalStatsDto implements IGlobalStatsDto {
    totalUsers?: number;
    totalQuizzesCreated?: number;
    averageQuizzesPerUser?: number;

    constructor(data?: IGlobalStatsDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(_data?: any) {
        if (_data) {
            this.totalUsers = _data["totalUsers"];
            this.totalQuizzesCreated = _data["totalQuizzesCreated"];
            this.averageQuizzesPerUser = _data["averageQuizzesPerUser"];
        }
    }

    static fromJS(data: any): GlobalStatsDto {
        data = typeof data === 'object' ? data : {};
        let result = new GlobalStatsDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["totalUsers"] = this.totalUsers;
        data["totalQuizzesCreated"] = this.totalQuizzesCreated;
        data["averageQuizzesPerUser"] = this.averageQuizzesPerUser;
        return data;
    }
}

export interface IGlobalStatsDto {
    totalUsers?: number;
    totalQuizzesCreated?: number;
    averageQuizzesPerUser?: number;
}

export class QuestionDto implements IQuestionDto {
    id!: string;
    text!: string;
    answers!: AnswerDto[];

    constructor(data?: IQuestionDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
        if (!data) {
            this.answers = [];
        }
    }

    init(_data?: any) {
        if (_data) {
            this.id = _data["id"];
            this.text = _data["text"];
            if (Array.isArray(_data["answers"])) {
                this.answers = [] as any;
                for (let item of _data["answers"])
                    this.answers!.push(AnswerDto.fromJS(item));
            }
        }
    }

    static fromJS(data: any): QuestionDto {
        data = typeof data === 'object' ? data : {};
        let result = new QuestionDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["id"] = this.id;
        data["text"] = this.text;
        if (Array.isArray(this.answers)) {
            data["answers"] = [];
            for (let item of this.answers)
                data["answers"].push(item.toJSON());
        }
        return data;
    }
}

export interface IQuestionDto {
    id: string;
    text: string;
    answers: AnswerDto[];
}

export class QuizAttemptDto implements IQuizAttemptDto {
    id!: string;
    startedAt!: Date;
    correctAnswers!: number;

    constructor(data?: IQuizAttemptDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(_data?: any) {
        if (_data) {
            this.id = _data["id"];
            this.startedAt = _data["startedAt"] ? new Date(_data["startedAt"].toString()) : <any>undefined;
            this.correctAnswers = _data["correctAnswers"];
        }
    }

    static fromJS(data: any): QuizAttemptDto {
        data = typeof data === 'object' ? data : {};
        let result = new QuizAttemptDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["id"] = this.id;
        data["startedAt"] = this.startedAt ? this.startedAt.toISOString() : <any>undefined;
        data["correctAnswers"] = this.correctAnswers;
        return data;
    }
}

export interface IQuizAttemptDto {
    id: string;
    startedAt: Date;
    correctAnswers: number;
}

export class QuizAttemptsDto implements IQuizAttemptsDto {
    name!: string;
    answers!: number;
    attempts!: QuizAttemptDto[];

    constructor(data?: IQuizAttemptsDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
        if (!data) {
            this.attempts = [];
        }
    }

    init(_data?: any) {
        if (_data) {
            this.name = _data["name"];
            this.answers = _data["answers"];
            if (Array.isArray(_data["attempts"])) {
                this.attempts = [] as any;
                for (let item of _data["attempts"])
                    this.attempts!.push(QuizAttemptDto.fromJS(item));
            }
        }
    }

    static fromJS(data: any): QuizAttemptsDto {
        data = typeof data === 'object' ? data : {};
        let result = new QuizAttemptsDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["name"] = this.name;
        data["answers"] = this.answers;
        if (Array.isArray(this.attempts)) {
            data["attempts"] = [];
            for (let item of this.attempts)
                data["attempts"].push(item.toJSON());
        }
        return data;
    }
}

export interface IQuizAttemptsDto {
    name: string;
    answers: number;
    attempts: QuizAttemptDto[];
}

export class QuizDto implements IQuizDto {
    id!: string;
    createdAt!: Date;
    title!: string;
    category!: Category;
    questionCount!: number;
    completedBy!: number;
    averageScore!: number;
    highScore!: number;
    isOwner!: boolean;

    constructor(data?: IQuizDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(_data?: any) {
        if (_data) {
            this.id = _data["id"];
            this.createdAt = _data["createdAt"] ? new Date(_data["createdAt"].toString()) : <any>undefined;
            this.title = _data["title"];
            this.category = _data["category"];
            this.questionCount = _data["questionCount"];
            this.completedBy = _data["completedBy"];
            this.averageScore = _data["averageScore"];
            this.highScore = _data["highScore"];
            this.isOwner = _data["isOwner"];
        }
    }

    static fromJS(data: any): QuizDto {
        data = typeof data === 'object' ? data : {};
        let result = new QuizDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["id"] = this.id;
        data["createdAt"] = this.createdAt ? this.createdAt.toISOString() : <any>undefined;
        data["title"] = this.title;
        data["category"] = this.category;
        data["questionCount"] = this.questionCount;
        data["completedBy"] = this.completedBy;
        data["averageScore"] = this.averageScore;
        data["highScore"] = this.highScore;
        data["isOwner"] = this.isOwner;
        return data;
    }
}

export interface IQuizDto {
    id: string;
    createdAt: Date;
    title: string;
    category: Category;
    questionCount: number;
    completedBy: number;
    averageScore: number;
    highScore: number;
    isOwner: boolean;
}

export class ValidatedAnswerDto implements IValidatedAnswerDto {
    selectedAnswer!: AnswerAttemptDto;
    correctAnswer!: AnswerDto;

    constructor(data?: IValidatedAnswerDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
        if (!data) {
            this.selectedAnswer = new AnswerAttemptDto();
            this.correctAnswer = new AnswerDto();
        }
    }

    init(_data?: any) {
        if (_data) {
            this.selectedAnswer = _data["selectedAnswer"] ? AnswerAttemptDto.fromJS(_data["selectedAnswer"]) : new AnswerAttemptDto();
            this.correctAnswer = _data["correctAnswer"] ? AnswerDto.fromJS(_data["correctAnswer"]) : new AnswerDto();
        }
    }

    static fromJS(data: any): ValidatedAnswerDto {
        data = typeof data === 'object' ? data : {};
        let result = new ValidatedAnswerDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["selectedAnswer"] = this.selectedAnswer ? this.selectedAnswer.toJSON() : <any>undefined;
        data["correctAnswer"] = this.correctAnswer ? this.correctAnswer.toJSON() : <any>undefined;
        return data;
    }
}

export interface IValidatedAnswerDto {
    selectedAnswer: AnswerAttemptDto;
    correctAnswer: AnswerDto;
}

export interface FileResponse {
    data: Blob;
    status: number;
    fileName?: string;
    headers?: { [name: string]: any };
}

export class ApiException extends Error {
    message: string;
    status: number;
    response: string;
    headers: { [key: string]: any; };
    result: any;

    constructor(message: string, status: number, response: string, headers: { [key: string]: any; }, result: any) {
        super();

        this.message = message;
        this.status = status;
        this.response = response;
        this.headers = headers;
        this.result = result;
    }

    protected isApiException = true;

    static isApiException(obj: any): obj is ApiException {
        return obj.isApiException === true;
    }
}

function throwException(message: string, status: number, response: string, headers: { [key: string]: any; }, result?: any): any {
    if (result !== null && result !== undefined)
        throw result;
    else
        throw new ApiException(message, status, response, headers, null);
}