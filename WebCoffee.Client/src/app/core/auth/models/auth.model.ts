export interface LoginRequest {
  tenDangNhap: string;
  matKhau: string;
}

export interface LoginResponse {
  accessToken: string;
  refreshToken: string;
}

export interface RefreshTokenRequest {
  refreshToken: string;
}

export interface UserPayload {
  maNV: string;
  tenDangNhap: string;
  hoTen: string;
  role: string;
  exp: number;
}