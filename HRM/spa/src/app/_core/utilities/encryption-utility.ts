import * as CryptoJS from 'crypto-js';

const ENCRYPTION_SECRET = 'Super secret key'
const secretPadded = ENCRYPTION_SECRET.padEnd(32, '0');
const secureKey = CryptoJS.enc.Utf8.parse(secretPadded.substring(0, 32));
const ivParameter = CryptoJS.enc.Utf8.parse(secretPadded.substring(0, 16));
const securityConfig = {
  iv: ivParameter,
  mode: CryptoJS.mode.CBC,
  padding: CryptoJS.pad.Pkcs7
};
export const encode = (input: any) => {
  if (input === null || input === undefined) return input;
  const inputData = CryptoJS.enc.Utf8.parse(
    typeof input === 'string' ? input : JSON.stringify(input)
  );
  const encrypted = CryptoJS.AES.encrypt(inputData, secureKey, securityConfig);
  return CryptoJS.enc.Base64.stringify(encrypted.ciphertext);
};
export const decode = (encodedInput: any) => {
  if (!encodedInput) return encodedInput;
  try {
    const encryptedData = CryptoJS.enc.Base64.parse(encodedInput);
    const decrypted = CryptoJS.AES.decrypt(
      <CryptoJS.lib.CipherParams>{ ciphertext: encryptedData },
      secureKey,
      securityConfig
    ).toString(CryptoJS.enc.Utf8);
    try {
      return JSON.parse(decrypted);
    } catch (_) {
      return decrypted;
    }
  } catch (_) {
    return encodedInput;
  }
};
