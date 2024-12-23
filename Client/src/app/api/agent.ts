import axios, { AxiosError, AxiosResponse } from "axios";
import { toast } from "react-toastify";

axios.defaults.baseURL = "http://localhost:5000/api/";
axios.defaults.withCredentials=true;

const sleep=()=>new Promise(resolve=>setTimeout(resolve,500));

const responseBody = (response: AxiosResponse) => response;

axios.interceptors.response.use(async (response) => {
await sleep();
    return response;
  },
  (error: AxiosError) => {
    const { data, status } = error.response as AxiosResponse;
    switch (status) {
      case 400:
        if(data.response){
          const modelStateErrors:string[]=[];
          for(const key in data.response){
            if(data.response[key]){
              modelStateErrors.push(data.response[key])
            }
          }
          console.log(modelStateErrors);

          throw modelStateErrors.flat();
        }
        toast.error(data.message);
        break;
        case 404:
        toast.error(data.message);
        break;
      case 401:
        toast.error(data.message);
        break;
      case 500:
        toast.error(data.message);
        break;
      default:
        break;
    }
    return Promise.reject(error.response);
  }
);

const requests = {
  get: (url: string) => axios.get(url).then(responseBody),
  post: (url: string, body: object) => axios.post(url, body).then(responseBody),
  put: (url: string, body: object) => axios.put(url, body).then(responseBody),
  delete: (url: string) => axios.delete(url).then(responseBody),
};

const Catalog = {
  list: () => requests.get("products/getproducts"),
  details: (id: number) => requests.get(`products/get/${id}`),
};

const TestError = {
  get400Error: () => requests.get("buggy/bad-request"),
  get401Error: () => requests.get("buggy/unauthorized"),
  get404Error: () => requests.get("buggy/not-found"),
  get500Error: () => requests.get("buggy/server-error"),
  getValidationError: () => requests.get("buggy/validation-error"),
};

const Basket={
  get:()=>requests.get('basket'),
  addItem:(productId:number,quantity=1)=>requests.post(`basket/additemtobasket?productId=${productId}&quantity=${quantity}`,{}),
  removeItem:(productId:number,quantity=1)=>requests.delete(`basket/removebasket?productId=${productId}&quantity=${quantity}`)
}

const agent = {
  Catalog,
  TestError,
  Basket
};
export default agent;
