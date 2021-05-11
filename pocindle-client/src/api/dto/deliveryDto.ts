//----------------------
// <auto-generated>
//     Generated using the NSwag toolchain v10.4.1.0 (Newtonsoft.Json v12.0.0.0) (http://NJsonSchema.org)
// </auto-generated>
//----------------------





export class DeliveryDto implements IDeliveryDto {
    userId!: number;
    to!: string;

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
            this.userId = _data["UserId"];
            this.to = _data["To"];
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
        data["UserId"] = this.userId;
        data["To"] = this.to;
        return data; 
    }
}

export interface IDeliveryDto {
    userId: number;
    to: string;
}
