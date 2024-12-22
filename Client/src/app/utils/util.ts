export function getCookie(key:string){
    const buyerId=document.cookie.match(`(^|;)\\s*${key}\\s*=\\s*([^;]+)`);
    return buyerId?buyerId.pop():"";
}