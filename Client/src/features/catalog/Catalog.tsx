import { Product } from "../../app/models/product";
import ProductList from "./ProductList";
import { useState, useEffect } from "react";
import agent from "../../app/api/agent";
import LoadingComponent from "../../app/layout/LoadingComponent";

export default function Catalog() {
  const [products, setProducts] = useState<Product[]>([]);
  const [loading, setLoading] = useState(true);
  useEffect(() => {
    agent.Catalog.list()
      .then((response) => {
        if (response.data.flag && response.data.statusCode === 200) {
          console.log("Product Details:", response.data.response);
          setProducts(response.data.response);
        } else {
          console.error("Error:", response.data.message);
        }
      })
      .catch((error) => console.log(error))
      .finally(() => setLoading(false));
  }, []);

  if (loading) return <LoadingComponent message="Loading products ..." />;
  return (
    <>
      <ProductList products={products}></ProductList>
    </>
  );
}
