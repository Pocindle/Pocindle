export enum StatusDto {
  Normal = 0,
  Archived = 1,
  ShouldBeDeleted = 2,
}

export interface IPocketItemDto {
  itemId: string;
  resolvedId: string;
  givenUrl: string;
  resolvedUrl: string;
  ampUrl: string | undefined;
  givenTitle: string;
  resolvedTitle: string;
  favorite: boolean;
  status: StatusDto;
  excerpt: string;
  isArticle: boolean;
  wordCount: number;
  listenDurationEstimate: number;
  timeToRead: number | undefined;
  timeAdded: Date;
  timeUpdated: Date;
}

export class PocketItemDto implements IPocketItemDto {
  itemId!: string;
  resolvedId!: string;
  givenUrl!: string;
  resolvedUrl!: string;
  ampUrl!: string | undefined;
  givenTitle!: string;
  resolvedTitle!: string;
  favorite!: boolean;
  status!: StatusDto;
  excerpt!: string;
  isArticle!: boolean;
  wordCount!: number;
  listenDurationEstimate!: number;
  timeToRead!: number | undefined;
  timeAdded!: Date;
  timeUpdated!: Date;

  constructor(data?: IPocketItemDto) {
    if (data) {
      for (const property in data) {
        if (Object.prototype.hasOwnProperty.call(data, property))
          (<any>this)[property] = (<any>data)[property];
      }
    }
  }

  init(_data?: any) {
    if (_data) {
      this.itemId = _data['ItemId'];
      this.resolvedId = _data['ResolvedId'];
      this.givenUrl = _data['GivenUrl'];
      this.resolvedUrl = _data['ResolvedUrl'];
      this.ampUrl = _data['AmpUrl'];
      this.givenTitle = _data['GivenTitle'];
      this.resolvedTitle = _data['ResolvedTitle'];
      this.favorite = _data['Favorite'];
      this.status = _data['Status'];
      this.excerpt = _data['Excerpt'];
      this.isArticle = _data['IsArticle'];
      this.wordCount = _data['WordCount'];
      this.listenDurationEstimate = _data['ListenDurationEstimate'];
      this.timeToRead = _data['TimeToRead'];
      this.timeAdded = _data['TimeAdded']
        ? new Date(_data['TimeAdded'].toString())
        : <any>undefined;
      this.timeUpdated = _data['TimeUpdated']
        ? new Date(_data['TimeUpdated'].toString())
        : <any>undefined;
    }
  }

  static fromJS(data: any): PocketItemDto {
    data = typeof data === 'object' ? data : {};
    const result = new PocketItemDto();
    result.init(data);
    return result;
  }

  toJSON(data?: any) {
    data = typeof data === 'object' ? data : {};
    data['ItemId'] = this.itemId;
    data['ResolvedId'] = this.resolvedId;
    data['GivenUrl'] = this.givenUrl;
    data['ResolvedUrl'] = this.resolvedUrl;
    data['AmpUrl'] = this.ampUrl;
    data['GivenTitle'] = this.givenTitle;
    data['ResolvedTitle'] = this.resolvedTitle;
    data['Favorite'] = this.favorite;
    data['Status'] = this.status;
    data['Excerpt'] = this.excerpt;
    data['IsArticle'] = this.isArticle;
    data['WordCount'] = this.wordCount;
    data['ListenDurationEstimate'] = this.listenDurationEstimate;
    data['TimeToRead'] = this.timeToRead;
    data['TimeAdded'] = this.timeAdded
      ? this.timeAdded.toISOString()
      : <any>undefined;
    data['TimeUpdated'] = this.timeUpdated
      ? this.timeUpdated.toISOString()
      : <any>undefined;
    return data;
  }
}