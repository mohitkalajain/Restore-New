import {
  Divider,
  Grid,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableRow,
  TextField,
  Typography,
} from "@mui/material";
import { ChangeEvent, useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { Product } from "../../app/models/product";
import agent from "../../app/api/agent";
import LoadingComponent from "../../app/layout/LoadingComponent";
import { useStoreContext } from "../../app/context/StoreContext";
import { LoadingButton } from "@mui/lab";

export default function ProductDetails() {
  const { basket,setBasket ,removeItem} = useStoreContext();
  const { id } = useParams<{ id: string }>();
  const [product, setProduct] = useState<Product | null>(null);
  const [loading, setLoading] = useState(true);

  const [quantity, setQuantity] = useState(0);
  const [submitting, setSubmitting] = useState(false);
  const item = basket?.items.find((x) => x.productId === product?.id);

  useEffect(() => {
    if (item) setQuantity(item.quantity);

    const parsedId = parseInt(id ?? "0");
    agent.Catalog.details(parsedId)
      .then((response) => {
        if (response.data.flag && response.data.statusCode === 200) {
          setProduct(response.data.response);
        } else {
          console.error("Error:", response.data.message);
        }
      })
      .catch((error) => console.log(error.response))
      .finally(() => setLoading(false));
  }, [id, item]);

  const handleInputChange=(event:ChangeEvent<HTMLInputElement>)=>{
    const value=Number(event.target.value);
    if(!isNaN(value)&& value>=0){
      setQuantity(value);
    } 
  }

const handleUpdateCart=()=>{
  setSubmitting(true);

  if(!product?.id){
    console.log('Invalid product ID')
    setSubmitting(false);
    return;
  }
  //check item not available 
  //check local quantity greater than the item inside basket 
  if(!item || quantity > item.quantity){
    const updatedQuantity=item?quantity-item.quantity:quantity;
agent.Basket.addItem(product?.id,updatedQuantity)
.then((basket) => {
  if (basket.data.flag && basket.data.statusCode === 200) {
    setBasket(basket.data.response);
  } else {
    console.error("Error:", basket.data.message);
  }
})
.catch((error) => console.log(error))
.finally(() => setSubmitting(false));

  }
  else{
    const updatedQuantity=item.quantity-quantity;
    
    agent.Basket.removeItem(product?.id, updatedQuantity)
      .then(() => removeItem(product?.id, updatedQuantity))
      .catch((error) => console.log(error))
      .finally(() => setSubmitting(false));
      
  }
}

  if (loading) return <LoadingComponent message="Loading product . . ." />;

  if (!product) return <h3>Product not found</h3>;

  return (
    <Grid container spacing={6}>
      <Grid item xs={6}>
        <img
          src={product?.pictureUrl}
          alt={product?.name}
          style={{ width: "100%" }}
        />
      </Grid>
      <Grid item xs={6}>
        <Typography variant="h3">{product?.name}</Typography>
        <Divider sx={{ mb: 2 }} />
        <Typography variant="h4" color="secondary">
          &#x20b9; {(product?.price / 100).toFixed(2)}
        </Typography>
        <TableContainer>
          <Table>
            <TableBody>
              <TableRow>
                <TableCell>Name</TableCell>
                <TableCell>{product?.name}</TableCell>
              </TableRow>

              <TableRow>
                <TableCell>Description</TableCell>
                <TableCell>{product?.description}</TableCell>
              </TableRow>

              <TableRow>
                <TableCell>Type</TableCell>
                <TableCell>{product?.type}</TableCell>
              </TableRow>

              <TableRow>
                <TableCell>Brand</TableCell>
                <TableCell>{product?.brand}</TableCell>
              </TableRow>
              <TableRow>
                <TableCell>Quantity in stock</TableCell>
                <TableCell>{product?.quantityInStock}</TableCell>
              </TableRow>
            </TableBody>
          </Table>
        </TableContainer>
        <Grid container spacing={2}>
          <Grid item xs={6}>
            <TextField
             onChange={handleInputChange}
              variant="outlined"
              type="number"
              label="Quantity in Cart"
              fullWidth
              value={quantity}
            />
          </Grid>
          <Grid item xs={6}>
            <LoadingButton
            disabled={item?.quantity===quantity || !item && quantity===0}
            loading={submitting}
            onClick={handleUpdateCart}
              sx={{ height: "55px" }}
              color="primary"
              size="large"
              variant="contained"
              fullWidth
            >
              {item ? "Update Quantity" : "Add to Cart"}
            </LoadingButton>
          </Grid>
        </Grid>
      </Grid>
    </Grid>
  );
}
