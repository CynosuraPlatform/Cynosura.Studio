import { AuthTokenModel } from './auth-tokens.model';

export interface AuthStateModel {
  tokens?: AuthTokenModel;
  authReady?: boolean;
}
