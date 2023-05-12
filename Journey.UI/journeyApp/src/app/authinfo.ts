import { isPlatform } from "@ionic/angular";
import config from "capacitor.config";

let authCallback = isPlatform('capacitor') ? `${config.appId}://dev-2mb38pu2.us.auth0.com/capacitor/${config.appId}/http://localhost:8100/auth-callback`
: 'http://localhost:8100/';

export const authInfo  = {
    domain: "dev-2mb38pu2.us.auth0.com",
    clientId: "qIz5NRX97DPzPwobltrLgy8U6nZTDW9w",
    useRefreshTokens: true,
    audience: 'iter-meum-api',
    useRefreshTokensFallback: false,
    authCallback:authCallback ,
    authorizationParams: {
      authCallbackUri: authCallback
    }
}