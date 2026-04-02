import { handleException } from "../exceptions/exception-handler.ts";
import { MethodNotAllowedException } from "../exceptions/method-not-allowed-exception.ts";

export interface RequestHandles {
    POST(req: Request) : Response | Promise<Response>;
    GET(req: Request) : Response | Promise<Response>;
    PUT(req: Request) : Response | Promise<Response>;
    DELETE(req: Request) : Response | Promise<Response>;
    PATCH(req: Request) : Response | Promise<Response>;
}

export class DefaultRequestHandler implements RequestHandles {
    protected static Instance(){
        return new DefaultRequestHandler();
    }

    protected constructor() {}

    public POST(_: Request): Response | Promise<Response> {
        throw new MethodNotAllowedException();
    }
    public GET(_: Request): Response | Promise<Response> {
         throw new MethodNotAllowedException();
    }
    public PUT(_: Request): Response | Promise<Response> {
        throw new MethodNotAllowedException();
    }
    public DELETE(_: Request): Response | Promise<Response> {
        throw new MethodNotAllowedException();
    }
    public PATCH(_: Request): Response | Promise<Response> {
        throw new MethodNotAllowedException();
    }

    public async invokeRequest(req: Request): Promise<Response> {
        try{
            return await this[req.method as keyof RequestHandles].call(this, req);
        }catch(err){
            return handleException(err);
        }
    }
}