//----------------------
// <auto-generated>
//     Generated using the NSwag toolchain v10.4.4.0 (Newtonsoft.Json v12.0.0.0) (http://NJsonSchema.org)
// </auto-generated>
//----------------------





export class RequestDto implements IRequestDto {
    requestToken!: string;
    redirectUrl!: string;

    constructor(data?: IRequestDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(_data?: any) {
        if (_data) {
            this.requestToken = _data["RequestToken"];
            this.redirectUrl = _data["RedirectUrl"];
        }
    }

    static fromJS(data: any): RequestDto {
        data = typeof data === 'object' ? data : {};
        let result = new RequestDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["RequestToken"] = this.requestToken;
        data["RedirectUrl"] = this.redirectUrl;
        return data; 
    }
}

export interface IRequestDto {
    requestToken: string;
    redirectUrl: string;
}
