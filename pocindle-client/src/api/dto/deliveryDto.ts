//----------------------
// <auto-generated>
//     Generated using the NSwag toolchain v10.4.4.0 (Newtonsoft.Json v12.0.0.0) (http://NJsonSchema.org)
// </auto-generated>
//----------------------





export class DeliveryDto implements IDeliveryDto {
    deliveryId!: number;
    userId!: number;
    articleUrl!: string;
    epubFile!: string;
    mobiFile!: string;
    to!: string | undefined;
    status!: boolean | undefined;
    statusMessage!: string | undefined;

    constructor(data?: IDeliveryDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(_data?: any) {
        if (_data) {
            this.deliveryId = _data["DeliveryId"];
            this.userId = _data["UserId"];
            this.articleUrl = _data["ArticleUrl"];
            this.epubFile = _data["EpubFile"];
            this.mobiFile = _data["MobiFile"];
            this.to = _data["To"];
            this.status = _data["Status"];
            this.statusMessage = _data["StatusMessage"];
        }
    }

    static fromJS(data: any): DeliveryDto {
        data = typeof data === 'object' ? data : {};
        let result = new DeliveryDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["DeliveryId"] = this.deliveryId;
        data["UserId"] = this.userId;
        data["ArticleUrl"] = this.articleUrl;
        data["EpubFile"] = this.epubFile;
        data["MobiFile"] = this.mobiFile;
        data["To"] = this.to;
        data["Status"] = this.status;
        data["StatusMessage"] = this.statusMessage;
        return data; 
    }
}

export interface IDeliveryDto {
    deliveryId: number;
    userId: number;
    articleUrl: string;
    epubFile: string;
    mobiFile: string;
    to: string | undefined;
    status: boolean | undefined;
    statusMessage: string | undefined;
}
