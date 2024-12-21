import {
  Avatar,
  Button,
  Card,
  CardActions,
  CardContent,
  CardHeader,
  CardMedia,
  Typography,
} from "@mui/material";
import { LoadingButton } from "@mui/lab";
import { Product } from "../../app/models/product";
import { Link } from "react-router-dom";
import { useState } from "react";
import agent from "../../app/api/agent";
interface Props {
  product: Product;
}
export default function ProductCard({ product }: Props) {
  const [loading, setLoading] = useState(false);

  const handleAddItem = (productId: number) => {
    setLoading(true);
    agent.Basket.addItem(productId)
      .catch((error) => console.log(error))
      .finally(() => setLoading(false));
  };

  return (
    <>
      <Card>
        <CardHeader
          avatar={
            <Avatar sx={{ bgcolor: "secondary.main" }}>
              {product.name.charAt(0).toUpperCase()}
            </Avatar>
          }
          title={product.name}
          titleTypographyProps={{
            sx: {
              fontWeight: "bold",
              color: "primary.main",
            },
          }}
        />
        <CardMedia
          sx={{
            height: 140,
            backgroundSize: "contain",
            bgcolor: "primary.main",
          }}
          image={product.pictureUrl}
          title={product.name}
        />
        <CardContent>
          <Typography gutterBottom variant="h5" color="secondary">
            &#x20b9; {(product.price / 100).toFixed(2)}
          </Typography>
          <Typography variant="body2" sx={{ color: "text.secondary" }}>
            {product.brand} / {product.type}
          </Typography>
        </CardContent>
        <CardActions>
          <LoadingButton
            loading={loading}
            onClick={() => handleAddItem(product.id)}
            size="small"
          >
            Add to cart
          </LoadingButton>
          <Button size="small" component={Link} to={`/catalog/${product.id}`}>
            View
          </Button>
        </CardActions>
      </Card>
    </>
  );
}
