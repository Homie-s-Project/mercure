---
title: Mercure API v1
language_tabs: []
language_clients: []
toc_footers: []
includes: []
search: false
highlight_theme: darkula
headingLevel: 2

---

<!-- Generator: Widdershins v4.0.1 -->

<h1 id="mercure-api">Mercure API v1</h1>

> Scroll down for example requests and responses.

# Authentication

* API Key (Bearer)
    - Parameter Name: **Authorization**, in: header. JWT Authorization header using the Bearer scheme. 

 Enter 'Bearer' [space] and then your token in the text input below.

Example: "Bearer 1safsfsdfdfd"

<h1 id="mercure-api-authentification">Authentification</h1>

## Permet de se connecter à l'applicaation

> Code samples

`GET /auth/login/google`

<h3 id="permet-de-se-connecter-à-l'applicaation-responses">Responses</h3>

|Status|Meaning|Description|Schema|
|---|---|---|---|
|200|[OK](https://tools.ietf.org/html/rfc7231#section-6.3.1)|Success|None|

<aside class="warning">
To perform this operation, you must be authenticated by means of one of the following methods:
Bearer
</aside>

## Reçois la requête de microsoft pour la connexion

> Code samples

`GET /auth/callback/microsoft`

<h3 id="reçois-la-requête-de-microsoft-pour-la-connexion-parameters">Parameters</h3>

|Name|In|Type|Required|Description|
|---|---|---|---|---|
|code|query|string|false|Le code de microsoft concernant la connexion|
|state|query|string|false|Le code qu'on à générer qui permet de protéger contre les Man In The Middle|
|error|query|string|false|Le code erreur s'il y en a une|
|errorDescription|query|string|false|La description de l'erreur|

<h3 id="reçois-la-requête-de-microsoft-pour-la-connexion-responses">Responses</h3>

|Status|Meaning|Description|Schema|
|---|---|---|---|
|200|[OK](https://tools.ietf.org/html/rfc7231#section-6.3.1)|Returns the token information|None|
|400|[Bad Request](https://tools.ietf.org/html/rfc7231#section-6.5.1)|Microsoft can't find your account, please contact the owner of the application|None|
|401|[Unauthorized](https://tools.ietf.org/html/rfc7235#section-3.1)|You are not authorize|None|
|404|[Not Found](https://tools.ietf.org/html/rfc7231#section-6.5.4)|Can't found your account|None|

<aside class="warning">
To perform this operation, you must be authenticated by means of one of the following methods:
Bearer
</aside>

## Reçois la requête de google pour la connexion

> Code samples

`GET /auth/callback/google`

<h3 id="reçois-la-requête-de-google-pour-la-connexion-parameters">Parameters</h3>

|Name|In|Type|Required|Description|
|---|---|---|---|---|
|code|query|string|false|Le code de microsoft concernant la connexion|
|state|query|string|false|Le code qu'on à générer qui permet de protéger contre les Man In The Middle|
|error|query|string|false|Le code erreur s'il y en a une|

<h3 id="reçois-la-requête-de-google-pour-la-connexion-responses">Responses</h3>

|Status|Meaning|Description|Schema|
|---|---|---|---|
|200|[OK](https://tools.ietf.org/html/rfc7231#section-6.3.1)|Returns the token information|None|
|400|[Bad Request](https://tools.ietf.org/html/rfc7231#section-6.5.1)|Google can't find your account, please contact the owner of the application|None|
|401|[Unauthorized](https://tools.ietf.org/html/rfc7235#section-3.1)|You are not authorize|None|
|404|[Not Found](https://tools.ietf.org/html/rfc7231#section-6.5.4)|Can't found your account|None|

<aside class="warning">
To perform this operation, you must be authenticated by means of one of the following methods:
Bearer
</aside>

## Return the token for the user saved in the cache with the state

> Code samples

`GET /auth/logged`

<h3 id="return-the-token-for-the-user-saved-in-the-cache-with-the-state-parameters">Parameters</h3>

|Name|In|Type|Required|Description|
|---|---|---|---|---|
|state|query|string|false|the state code generated when the user logged|

> Example responses

> 200 Response

```json
{
  "userId": 0,
  "lastName": "string",
  "firstName": "string",
  "email": "string"
}
```

<h3 id="return-the-token-for-the-user-saved-in-the-cache-with-the-state-responses">Responses</h3>

|Status|Meaning|Description|Schema|
|---|---|---|---|
|200|[OK](https://tools.ietf.org/html/rfc7231#section-6.3.1)|Success|[UserDto](#schemauserdto)|
|404|[Not Found](https://tools.ietf.org/html/rfc7231#section-6.5.4)|Not Found|[ErrorMessage](#schemaerrormessage)|

<aside class="warning">
To perform this operation, you must be authenticated by means of one of the following methods:
Bearer
</aside>

## Return the current user connected

> Code samples

`GET /auth/current-user`

> Example responses

> 200 Response

```json
{
  "userId": 0,
  "lastName": "string",
  "firstName": "string",
  "email": "string"
}
```

<h3 id="return-the-current-user-connected-responses">Responses</h3>

|Status|Meaning|Description|Schema|
|---|---|---|---|
|200|[OK](https://tools.ietf.org/html/rfc7231#section-6.3.1)|Success|[UserDto](#schemauserdto)|
|404|[Not Found](https://tools.ietf.org/html/rfc7231#section-6.5.4)|Not Found|[ErrorMessage](#schemaerrormessage)|

<aside class="warning">
To perform this operation, you must be authenticated by means of one of the following methods:
Bearer
</aside>

<h1 id="mercure-api-cart">Cart</h1>

## Get the cart of the user

> Code samples

`GET /cart`

<h3 id="get-the-cart-of-the-user-parameters">Parameters</h3>

|Name|In|Type|Required|Description|
|---|---|---|---|---|
|randomId|query|string|false|none|

<h3 id="get-the-cart-of-the-user-responses">Responses</h3>

|Status|Meaning|Description|Schema|
|---|---|---|---|
|200|[OK](https://tools.ietf.org/html/rfc7231#section-6.3.1)|Success|None|

<aside class="warning">
To perform this operation, you must be authenticated by means of one of the following methods:
Bearer
</aside>

## Add a product to the cart

> Code samples

`POST /add/{productId}`

<h3 id="add-a-product-to-the-cart-parameters">Parameters</h3>

|Name|In|Type|Required|Description|
|---|---|---|---|---|
|productId|path|string|true|none|
|randomId|query|string|false|none|
|quantity|query|string|false|none|

<h3 id="add-a-product-to-the-cart-responses">Responses</h3>

|Status|Meaning|Description|Schema|
|---|---|---|---|
|200|[OK](https://tools.ietf.org/html/rfc7231#section-6.3.1)|Success|None|

<aside class="warning">
To perform this operation, you must be authenticated by means of one of the following methods:
Bearer
</aside>

## Remove a product from the cart

> Code samples

`DELETE /remove/{productId}`

<h3 id="remove-a-product-from-the-cart-parameters">Parameters</h3>

|Name|In|Type|Required|Description|
|---|---|---|---|---|
|productId|path|string|true|none|
|randomId|query|string|false|none|

<h3 id="remove-a-product-from-the-cart-responses">Responses</h3>

|Status|Meaning|Description|Schema|
|---|---|---|---|
|200|[OK](https://tools.ietf.org/html/rfc7231#section-6.3.1)|Success|None|

<aside class="warning">
To perform this operation, you must be authenticated by means of one of the following methods:
Bearer
</aside>

<h1 id="mercure-api-product">Product</h1>

## Get the product for the given idq

> Code samples

`GET /products/{productId}`

<h3 id="get-the-product-for-the-given-idq-parameters">Parameters</h3>

|Name|In|Type|Required|Description|
|---|---|---|---|---|
|productId|path|string|true|none|

> Example responses

> 200 Response

```json
{
  "productId": 0,
  "productBrandName": "string",
  "productName": "string",
  "productDescription": "string",
  "productPrice": 0,
  "productCreationDate": "2019-08-24T14:15:22Z",
  "productLastUpdate": "2019-08-24T14:15:22Z",
  "stock": {
    "quantity": 0
  },
  "categories": [
    {
      "categoryTitle": "string",
      "categoryDescription": "string",
      "products": [
        {
          "productId": 0,
          "productBrandName": "string",
          "productName": "string",
          "productDescription": "string",
          "productPrice": 0,
          "productCreationDate": "2019-08-24T14:15:22Z",
          "productLastUpdate": "2019-08-24T14:15:22Z",
          "stock": {
            "quantity": 0
          },
          "categories": []
        }
      ]
    }
  ]
}
```

<h3 id="get-the-product-for-the-given-idq-responses">Responses</h3>

|Status|Meaning|Description|Schema|
|---|---|---|---|
|200|[OK](https://tools.ietf.org/html/rfc7231#section-6.3.1)|Success|[ProductDto](#schemaproductdto)|
|400|[Bad Request](https://tools.ietf.org/html/rfc7231#section-6.5.1)|Bad Request|[ErrorMessage](#schemaerrormessage)|
|401|[Unauthorized](https://tools.ietf.org/html/rfc7235#section-3.1)|Unauthorized|[ErrorMessage](#schemaerrormessage)|

<aside class="warning">
To perform this operation, you must be authenticated by means of one of the following methods:
Bearer
</aside>

## Creates a new product.

> Code samples

`POST /products/create`

> Body parameter

```yaml
productName: string
productBrandName: string
productDescription: string
productPrice: 0
stockId: 0
categories: string

```

<h3 id="creates-a-new-product.-parameters">Parameters</h3>

|Name|In|Type|Required|Description|
|---|---|---|---|---|
|body|body|object|false|none|
|» productName|body|string|false|none|
|» productBrandName|body|string|false|none|
|» productDescription|body|string|false|none|
|» productPrice|body|integer(int32)|false|none|
|» stockId|body|integer(int32)|false|none|
|» categories|body|string|false|none|

> Example responses

> 200 Response

```json
{
  "productId": 0,
  "productBrandName": "string",
  "productName": "string",
  "productDescription": "string",
  "productPrice": 0,
  "productCreationDate": "2019-08-24T14:15:22Z",
  "productLastUpdate": "2019-08-24T14:15:22Z",
  "stock": {
    "quantity": 0
  },
  "categories": [
    {
      "categoryTitle": "string",
      "categoryDescription": "string",
      "products": [
        {
          "productId": 0,
          "productBrandName": "string",
          "productName": "string",
          "productDescription": "string",
          "productPrice": 0,
          "productCreationDate": "2019-08-24T14:15:22Z",
          "productLastUpdate": "2019-08-24T14:15:22Z",
          "stock": {
            "quantity": 0
          },
          "categories": []
        }
      ]
    }
  ]
}
```

<h3 id="creates-a-new-product.-responses">Responses</h3>

|Status|Meaning|Description|Schema|
|---|---|---|---|
|200|[OK](https://tools.ietf.org/html/rfc7231#section-6.3.1)|Success|[ProductDto](#schemaproductdto)|
|400|[Bad Request](https://tools.ietf.org/html/rfc7231#section-6.5.1)|Bad Request|[ErrorMessage](#schemaerrormessage)|
|401|[Unauthorized](https://tools.ietf.org/html/rfc7235#section-3.1)|Unauthorized|[ErrorMessage](#schemaerrormessage)|

<aside class="warning">
To perform this operation, you must be authenticated by means of one of the following methods:
Bearer
</aside>

## Updates an existing product.

> Code samples

`PUT /products/update/{productId}`

> Body parameter

```yaml
productName: string
productBrandName: string
productDescription: string
productPrice: 0
stockId: 0
categories: string

```

<h3 id="updates-an-existing-product.-parameters">Parameters</h3>

|Name|In|Type|Required|Description|
|---|---|---|---|---|
|productId|path|string|true|The identifier of the product to update.|
|body|body|object|false|none|
|» productName|body|string|false|none|
|» productBrandName|body|string|false|none|
|» productDescription|body|string|false|none|
|» productPrice|body|integer(int32)|false|none|
|» stockId|body|integer(int32)|false|none|
|» categories|body|string|false|none|

> Example responses

> 200 Response

```json
{
  "productId": 0,
  "productBrandName": "string",
  "productName": "string",
  "productDescription": "string",
  "productPrice": 0,
  "productCreationDate": "2019-08-24T14:15:22Z",
  "productLastUpdate": "2019-08-24T14:15:22Z",
  "stock": {
    "quantity": 0
  },
  "categories": [
    {
      "categoryTitle": "string",
      "categoryDescription": "string",
      "products": [
        {
          "productId": 0,
          "productBrandName": "string",
          "productName": "string",
          "productDescription": "string",
          "productPrice": 0,
          "productCreationDate": "2019-08-24T14:15:22Z",
          "productLastUpdate": "2019-08-24T14:15:22Z",
          "stock": {
            "quantity": 0
          },
          "categories": []
        }
      ]
    }
  ]
}
```

<h3 id="updates-an-existing-product.-responses">Responses</h3>

|Status|Meaning|Description|Schema|
|---|---|---|---|
|200|[OK](https://tools.ietf.org/html/rfc7231#section-6.3.1)|Success|[ProductDto](#schemaproductdto)|
|400|[Bad Request](https://tools.ietf.org/html/rfc7231#section-6.5.1)|Bad Request|[ErrorMessage](#schemaerrormessage)|
|401|[Unauthorized](https://tools.ietf.org/html/rfc7235#section-3.1)|Unauthorized|[ErrorMessage](#schemaerrormessage)|

<aside class="warning">
To perform this operation, you must be authenticated by means of one of the following methods:
Bearer
</aside>

## delete__products_delete_{productId}

> Code samples

`DELETE /products/delete/{productId}`

<h3 id="delete__products_delete_{productid}-parameters">Parameters</h3>

|Name|In|Type|Required|Description|
|---|---|---|---|---|
|productId|path|string|true|none|

> Example responses

> 200 Response

```json
{
  "message": "string",
  "statuCode": 0
}
```

<h3 id="delete__products_delete_{productid}-responses">Responses</h3>

|Status|Meaning|Description|Schema|
|---|---|---|---|
|200|[OK](https://tools.ietf.org/html/rfc7231#section-6.3.1)|Success|[ErrorMessage](#schemaerrormessage)|
|400|[Bad Request](https://tools.ietf.org/html/rfc7231#section-6.5.1)|Bad Request|[ErrorMessage](#schemaerrormessage)|
|401|[Unauthorized](https://tools.ietf.org/html/rfc7235#section-3.1)|Unauthorized|[ErrorMessage](#schemaerrormessage)|

<aside class="warning">
To perform this operation, you must be authenticated by means of one of the following methods:
Bearer
</aside>

<h1 id="mercure-api-shopping">Shopping</h1>

## Get random products for the home page

> Code samples

`GET /shopping`

> Example responses

> 200 Response

```json
[
  {
    "productId": 0,
    "productBrandName": "string",
    "productName": "string",
    "productDescription": "string",
    "productPrice": 0,
    "productCreationDate": "2019-08-24T14:15:22Z",
    "productLastUpdate": "2019-08-24T14:15:22Z",
    "stockId": 0,
    "stock": {
      "stockId": 0,
      "stockQuantityAvailable": 0
    },
    "categories": [
      {
        "categoryId": 0,
        "categoryTitle": "string",
        "categoryDescription": "string",
        "products": [
          {}
        ]
      }
    ]
  }
]
```

<h3 id="get-random-products-for-the-home-page-responses">Responses</h3>

|Status|Meaning|Description|Schema|
|---|---|---|---|
|200|[OK](https://tools.ietf.org/html/rfc7231#section-6.3.1)|Success|Inline|
|404|[Not Found](https://tools.ietf.org/html/rfc7231#section-6.5.4)|Not Found|[ErrorMessage](#schemaerrormessage)|

<h3 id="get-random-products-for-the-home-page-responseschema">Response Schema</h3>

Status Code **200**

|Name|Type|Required|Restrictions|Description|
|---|---|---|---|---|
|*anonymous*|[[Product](#schemaproduct)]|false|none|[Product model]|
|» productId|integer(int32)|false|none|none|
|» productBrandName|string¦null|false|none|none|
|» productName|string¦null|false|none|none|
|» productDescription|string¦null|false|none|none|
|» productPrice|integer(int32)|false|none|none|
|» productCreationDate|string(date-time)|false|none|none|
|» productLastUpdate|string(date-time)|false|none|none|
|» stockId|integer(int32)|false|none|none|
|» stock|[Stock](#schemastock)|false|none|none|
|»» stockId|integer(int32)|false|none|none|
|»» stockQuantityAvailable|integer(int32)|false|none|none|
|» categories|[[Category](#schemacategory)]¦null|false|none|none|
|»» categoryId|integer(int32)|false|none|none|
|»» categoryTitle|string¦null|false|none|none|
|»» categoryDescription|string¦null|false|none|none|
|»» products|[[Product](#schemaproduct)]¦null|false|none|Product model|

<aside class="warning">
To perform this operation, you must be authenticated by means of one of the following methods:
Bearer
</aside>

## Get the best seller products

> Code samples

`GET /shopping/bestSeller`

> Example responses

> 200 Response

```json
[
  {
    "orderProductId": 0,
    "productId": 0,
    "product": {
      "productId": 0,
      "productBrandName": "string",
      "productName": "string",
      "productDescription": "string",
      "productPrice": 0,
      "productCreationDate": "2019-08-24T14:15:22Z",
      "productLastUpdate": "2019-08-24T14:15:22Z",
      "stockId": 0,
      "stock": {
        "stockId": 0,
        "stockQuantityAvailable": 0
      },
      "categories": [
        {
          "categoryId": 0,
          "categoryTitle": "string",
          "categoryDescription": "string",
          "products": [
            {}
          ]
        }
      ]
    },
    "orderId": 0,
    "order": {
      "orderId": 0,
      "orderPrice": 0,
      "orderTaxPrice": 0,
      "orderDeliveryPrice": 0,
      "orderDate": "2019-08-24T14:15:22Z",
      "orderStatus": true,
      "userId": 0,
      "user": {
        "userId": 0,
        "serviceId": "string",
        "lastName": "string",
        "firstName": "string",
        "birthDate": "2019-08-24T14:15:22Z",
        "email": "string",
        "createdAt": "2019-08-24T14:15:22Z",
        "lastUpdatedAt": "2019-08-24T14:15:22Z",
        "roleId": 0,
        "role": {
          "roleId": 0,
          "roleName": "string",
          "roleNumber": 0
        },
        "orders": [
          {}
        ]
      },
      "products": [
        {}
      ],
      "animals": [
        {
          "animalId": 0,
          "animalBirthDate": "2019-08-24T14:15:22Z",
          "animalColor": "string",
          "animalPrice": 0,
          "animalCreationDate": "2019-08-24T14:15:22Z",
          "animalLastUpdate": "2019-08-24T14:15:22Z",
          "speciesId": 0,
          "species": {
            "speciesId": 0,
            "speciesName": "string"
          }
        }
      ]
    },
    "quantity": 0,
    "shipped": true
  }
]
```

<h3 id="get-the-best-seller-products-responses">Responses</h3>

|Status|Meaning|Description|Schema|
|---|---|---|---|
|200|[OK](https://tools.ietf.org/html/rfc7231#section-6.3.1)|Success|Inline|
|404|[Not Found](https://tools.ietf.org/html/rfc7231#section-6.5.4)|Not Found|[ErrorMessage](#schemaerrormessage)|

<h3 id="get-the-best-seller-products-responseschema">Response Schema</h3>

Status Code **200**

|Name|Type|Required|Restrictions|Description|
|---|---|---|---|---|
|*anonymous*|[[OrderProduct](#schemaorderproduct)]|false|none|none|
|» orderProductId|integer(int32)|false|none|none|
|» productId|integer(int32)|false|none|none|
|» product|[Product](#schemaproduct)|false|none|Product model|
|»» productId|integer(int32)|false|none|none|
|»» productBrandName|string¦null|false|none|none|
|»» productName|string¦null|false|none|none|
|»» productDescription|string¦null|false|none|none|
|»» productPrice|integer(int32)|false|none|none|
|»» productCreationDate|string(date-time)|false|none|none|
|»» productLastUpdate|string(date-time)|false|none|none|
|»» stockId|integer(int32)|false|none|none|
|»» stock|[Stock](#schemastock)|false|none|none|
|»»» stockId|integer(int32)|false|none|none|
|»»» stockQuantityAvailable|integer(int32)|false|none|none|
|»» categories|[[Category](#schemacategory)]¦null|false|none|none|
|»»» categoryId|integer(int32)|false|none|none|
|»»» categoryTitle|string¦null|false|none|none|
|»»» categoryDescription|string¦null|false|none|none|
|»»» products|[[Product](#schemaproduct)]¦null|false|none|Product model|
|» orderId|integer(int32)|false|none|none|
|» order|[Order](#schemaorder)|false|none|none|
|»» orderId|integer(int32)|false|none|none|
|»» orderPrice|integer(int32)|false|none|none|
|»» orderTaxPrice|integer(int32)|false|none|none|
|»» orderDeliveryPrice|integer(int32)|false|none|none|
|»» orderDate|string(date-time)|false|none|none|
|»» orderStatus|boolean|false|none|none|
|»» userId|integer(int32)|false|none|none|
|»» user|[User](#schemauser)|false|none|none|
|»»» userId|integer(int32)|false|none|none|
|»»» serviceId|string|true|none|none|
|»»» lastName|string¦null|false|none|none|
|»»» firstName|string¦null|false|none|none|
|»»» birthDate|string(date-time)¦null|false|none|none|
|»»» email|string|true|none|none|
|»»» createdAt|string(date-time)|false|none|none|
|»»» lastUpdatedAt|string(date-time)|false|none|none|
|»»» roleId|integer(int32)|false|none|none|
|»»» role|[Role](#schemarole)|false|none|none|
|»»»» roleId|integer(int32)|false|none|none|
|»»»» roleName|string¦null|false|none|none|
|»»»» roleNumber|integer(int32)|false|none|none|
|»»» orders|[[Order](#schemaorder)]¦null|false|none|none|
|»» products|[[OrderProduct](#schemaorderproduct)]¦null|false|none|none|
|»» animals|[[Animal](#schemaanimal)]¦null|false|none|none|
|»»» animalId|integer(int32)|false|none|none|
|»»» animalBirthDate|string(date-time)|false|none|none|
|»»» animalColor|string¦null|false|none|none|
|»»» animalPrice|integer(int32)|false|none|none|
|»»» animalCreationDate|string(date-time)|false|none|none|
|»»» animalLastUpdate|string(date-time)|false|none|none|
|»»» speciesId|integer(int32)|false|none|none|
|»»» species|[Species](#schemaspecies)|false|none|none|
|»»»» speciesId|integer(int32)|false|none|none|
|»»»» speciesName|string¦null|false|none|none|
|» quantity|integer(int32)|false|none|none|
|» shipped|boolean|false|none|none|

<aside class="warning">
To perform this operation, you must be authenticated by means of one of the following methods:
Bearer
</aside>

## Get the brands of the products

> Code samples

`GET /shopping/brands`

> Example responses

> 200 Response

```json
[
  "string"
]
```

<h3 id="get-the-brands-of-the-products-responses">Responses</h3>

|Status|Meaning|Description|Schema|
|---|---|---|---|
|200|[OK](https://tools.ietf.org/html/rfc7231#section-6.3.1)|Success|Inline|
|404|[Not Found](https://tools.ietf.org/html/rfc7231#section-6.5.4)|Not Found|[ErrorMessage](#schemaerrormessage)|

<h3 id="get-the-brands-of-the-products-responseschema">Response Schema</h3>

<aside class="warning">
To perform this operation, you must be authenticated by means of one of the following methods:
Bearer
</aside>

## Get the categories of the products

> Code samples

`GET /shopping/categories`

> Example responses

> 200 Response

```json
[
  {
    "categoryId": 0,
    "categoryTitle": "string",
    "categoryDescription": "string",
    "products": [
      {
        "productId": 0,
        "productBrandName": "string",
        "productName": "string",
        "productDescription": "string",
        "productPrice": 0,
        "productCreationDate": "2019-08-24T14:15:22Z",
        "productLastUpdate": "2019-08-24T14:15:22Z",
        "stockId": 0,
        "stock": {
          "stockId": 0,
          "stockQuantityAvailable": 0
        },
        "categories": [
          {}
        ]
      }
    ]
  }
]
```

<h3 id="get-the-categories-of-the-products-responses">Responses</h3>

|Status|Meaning|Description|Schema|
|---|---|---|---|
|200|[OK](https://tools.ietf.org/html/rfc7231#section-6.3.1)|Success|Inline|
|404|[Not Found](https://tools.ietf.org/html/rfc7231#section-6.5.4)|Not Found|[ErrorMessage](#schemaerrormessage)|

<h3 id="get-the-categories-of-the-products-responseschema">Response Schema</h3>

Status Code **200**

|Name|Type|Required|Restrictions|Description|
|---|---|---|---|---|
|*anonymous*|[[Category](#schemacategory)]|false|none|none|
|» categoryId|integer(int32)|false|none|none|
|» categoryTitle|string¦null|false|none|none|
|» categoryDescription|string¦null|false|none|none|
|» products|[[Product](#schemaproduct)]¦null|false|none|Product model|
|»» productId|integer(int32)|false|none|none|
|»» productBrandName|string¦null|false|none|none|
|»» productName|string¦null|false|none|none|
|»» productDescription|string¦null|false|none|none|
|»» productPrice|integer(int32)|false|none|none|
|»» productCreationDate|string(date-time)|false|none|none|
|»» productLastUpdate|string(date-time)|false|none|none|
|»» stockId|integer(int32)|false|none|none|
|»» stock|[Stock](#schemastock)|false|none|none|
|»»» stockId|integer(int32)|false|none|none|
|»»» stockQuantityAvailable|integer(int32)|false|none|none|
|»» categories|[[Category](#schemacategory)]¦null|false|none|none|

<aside class="warning">
To perform this operation, you must be authenticated by means of one of the following methods:
Bearer
</aside>

## Get result from a search

> Code samples

`GET /shopping/search/{search}`

<h3 id="get-result-from-a-search-parameters">Parameters</h3>

|Name|In|Type|Required|Description|
|---|---|---|---|---|
|search|path|string|true|none|
|brand|query|string|false|none|
|category|query|string|false|none|
|minPrice|query|string|false|none|
|maxPrice|query|string|false|none|

> Example responses

> 200 Response

```json
[
  {
    "productId": 0,
    "productBrandName": "string",
    "productName": "string",
    "productDescription": "string",
    "productPrice": 0,
    "productCreationDate": "2019-08-24T14:15:22Z",
    "productLastUpdate": "2019-08-24T14:15:22Z",
    "stockId": 0,
    "stock": {
      "stockId": 0,
      "stockQuantityAvailable": 0
    },
    "categories": [
      {
        "categoryId": 0,
        "categoryTitle": "string",
        "categoryDescription": "string",
        "products": [
          {}
        ]
      }
    ]
  }
]
```

<h3 id="get-result-from-a-search-responses">Responses</h3>

|Status|Meaning|Description|Schema|
|---|---|---|---|
|200|[OK](https://tools.ietf.org/html/rfc7231#section-6.3.1)|Success|Inline|
|404|[Not Found](https://tools.ietf.org/html/rfc7231#section-6.5.4)|Not Found|[ErrorMessage](#schemaerrormessage)|

<h3 id="get-result-from-a-search-responseschema">Response Schema</h3>

Status Code **200**

|Name|Type|Required|Restrictions|Description|
|---|---|---|---|---|
|*anonymous*|[[Product](#schemaproduct)]|false|none|[Product model]|
|» productId|integer(int32)|false|none|none|
|» productBrandName|string¦null|false|none|none|
|» productName|string¦null|false|none|none|
|» productDescription|string¦null|false|none|none|
|» productPrice|integer(int32)|false|none|none|
|» productCreationDate|string(date-time)|false|none|none|
|» productLastUpdate|string(date-time)|false|none|none|
|» stockId|integer(int32)|false|none|none|
|» stock|[Stock](#schemastock)|false|none|none|
|»» stockId|integer(int32)|false|none|none|
|»» stockQuantityAvailable|integer(int32)|false|none|none|
|» categories|[[Category](#schemacategory)]¦null|false|none|none|
|»» categoryId|integer(int32)|false|none|none|
|»» categoryTitle|string¦null|false|none|none|
|»» categoryDescription|string¦null|false|none|none|
|»» products|[[Product](#schemaproduct)]¦null|false|none|Product model|

<aside class="warning">
To perform this operation, you must be authenticated by means of one of the following methods:
Bearer
</aside>

<h1 id="mercure-api-test">Test</h1>

## Route for test if user is connected

> Code samples

`GET /dev/test-connected`

<h3 id="route-for-test-if-user-is-connected-responses">Responses</h3>

|Status|Meaning|Description|Schema|
|---|---|---|---|
|200|[OK](https://tools.ietf.org/html/rfc7231#section-6.3.1)|Success|None|

<aside class="warning">
To perform this operation, you must be authenticated by means of one of the following methods:
Bearer
</aside>

## Route for test if user database has dev user

> Code samples

`GET /dev/test-dev-users`

<h3 id="route-for-test-if-user-database-has-dev-user-responses">Responses</h3>

|Status|Meaning|Description|Schema|
|---|---|---|---|
|200|[OK](https://tools.ietf.org/html/rfc7231#section-6.3.1)|Success|None|

<aside class="warning">
To perform this operation, you must be authenticated by means of one of the following methods:
Bearer
</aside>

## Route that return all dev users to test the roles

> Code samples

`GET /dev/test-roles`

> Example responses

> 200 Response

```json
{
  "roleName": "string",
  "token": "string"
}
```

<h3 id="route-that-return-all-dev-users-to-test-the-roles-responses">Responses</h3>

|Status|Meaning|Description|Schema|
|---|---|---|---|
|200|[OK](https://tools.ietf.org/html/rfc7231#section-6.3.1)|Success|[UserRole](#schemauserrole)|
|400|[Bad Request](https://tools.ietf.org/html/rfc7231#section-6.5.1)|Bad Request|[ErrorMessage](#schemaerrormessage)|

<aside class="warning">
To perform this operation, you must be authenticated by means of one of the following methods:
Bearer
</aside>

## Get all roles for dev

> Code samples

`GET /dev/roles`

<h3 id="get-all-roles-for-dev-responses">Responses</h3>

|Status|Meaning|Description|Schema|
|---|---|---|---|
|200|[OK](https://tools.ietf.org/html/rfc7231#section-6.3.1)|Success|None|

<aside class="warning">
To perform this operation, you must be authenticated by means of one of the following methods:
Bearer
</aside>

## Set roles for current user, has to be conneceted

> Code samples

`POST /dev/roles/{roleNumber}`

<h3 id="set-roles-for-current-user,-has-to-be-conneceted-parameters">Parameters</h3>

|Name|In|Type|Required|Description|
|---|---|---|---|---|
|roleNumber|path|string|true|none|

<h3 id="set-roles-for-current-user,-has-to-be-conneceted-responses">Responses</h3>

|Status|Meaning|Description|Schema|
|---|---|---|---|
|200|[OK](https://tools.ietf.org/html/rfc7231#section-6.3.1)|Success|None|

<aside class="warning">
To perform this operation, you must be authenticated by means of one of the following methods:
Bearer
</aside>

# Schemas

<h2 id="tocS_Animal">Animal</h2>
<!-- backwards compatibility -->
<a id="schemaanimal"></a>
<a id="schema_Animal"></a>
<a id="tocSanimal"></a>
<a id="tocsanimal"></a>

```json
{
  "animalId": 0,
  "animalBirthDate": "2019-08-24T14:15:22Z",
  "animalColor": "string",
  "animalPrice": 0,
  "animalCreationDate": "2019-08-24T14:15:22Z",
  "animalLastUpdate": "2019-08-24T14:15:22Z",
  "speciesId": 0,
  "species": {
    "speciesId": 0,
    "speciesName": "string"
  }
}

```

### Properties

|Name|Type|Required|Restrictions|Description|
|---|---|---|---|---|
|animalId|integer(int32)|false|none|none|
|animalBirthDate|string(date-time)|false|none|none|
|animalColor|string¦null|false|none|none|
|animalPrice|integer(int32)|false|none|none|
|animalCreationDate|string(date-time)|false|none|none|
|animalLastUpdate|string(date-time)|false|none|none|
|speciesId|integer(int32)|false|none|none|
|species|[Species](#schemaspecies)|false|none|none|

<h2 id="tocS_Category">Category</h2>
<!-- backwards compatibility -->
<a id="schemacategory"></a>
<a id="schema_Category"></a>
<a id="tocScategory"></a>
<a id="tocscategory"></a>

```json
{
  "categoryId": 0,
  "categoryTitle": "string",
  "categoryDescription": "string",
  "products": [
    {
      "productId": 0,
      "productBrandName": "string",
      "productName": "string",
      "productDescription": "string",
      "productPrice": 0,
      "productCreationDate": "2019-08-24T14:15:22Z",
      "productLastUpdate": "2019-08-24T14:15:22Z",
      "stockId": 0,
      "stock": {
        "stockId": 0,
        "stockQuantityAvailable": 0
      },
      "categories": [
        {
          "categoryId": 0,
          "categoryTitle": "string",
          "categoryDescription": "string",
          "products": []
        }
      ]
    }
  ]
}

```

### Properties

|Name|Type|Required|Restrictions|Description|
|---|---|---|---|---|
|categoryId|integer(int32)|false|none|none|
|categoryTitle|string¦null|false|none|none|
|categoryDescription|string¦null|false|none|none|
|products|[[Product](#schemaproduct)]¦null|false|none|[Product model]|

<h2 id="tocS_CategoryDto">CategoryDto</h2>
<!-- backwards compatibility -->
<a id="schemacategorydto"></a>
<a id="schema_CategoryDto"></a>
<a id="tocScategorydto"></a>
<a id="tocscategorydto"></a>

```json
{
  "categoryTitle": "string",
  "categoryDescription": "string",
  "products": [
    {
      "productId": 0,
      "productBrandName": "string",
      "productName": "string",
      "productDescription": "string",
      "productPrice": 0,
      "productCreationDate": "2019-08-24T14:15:22Z",
      "productLastUpdate": "2019-08-24T14:15:22Z",
      "stock": {
        "quantity": 0
      },
      "categories": [
        {
          "categoryTitle": "string",
          "categoryDescription": "string",
          "products": []
        }
      ]
    }
  ]
}

```

### Properties

|Name|Type|Required|Restrictions|Description|
|---|---|---|---|---|
|categoryTitle|string¦null|false|none|none|
|categoryDescription|string¦null|false|none|none|
|products|[[ProductDto](#schemaproductdto)]¦null|false|none|none|

<h2 id="tocS_ErrorMessage">ErrorMessage</h2>
<!-- backwards compatibility -->
<a id="schemaerrormessage"></a>
<a id="schema_ErrorMessage"></a>
<a id="tocSerrormessage"></a>
<a id="tocserrormessage"></a>

```json
{
  "message": "string",
  "statuCode": 0
}

```

### Properties

|Name|Type|Required|Restrictions|Description|
|---|---|---|---|---|
|message|string¦null|false|none|none|
|statuCode|integer(int32)|false|none|none|

<h2 id="tocS_Order">Order</h2>
<!-- backwards compatibility -->
<a id="schemaorder"></a>
<a id="schema_Order"></a>
<a id="tocSorder"></a>
<a id="tocsorder"></a>

```json
{
  "orderId": 0,
  "orderPrice": 0,
  "orderTaxPrice": 0,
  "orderDeliveryPrice": 0,
  "orderDate": "2019-08-24T14:15:22Z",
  "orderStatus": true,
  "userId": 0,
  "user": {
    "userId": 0,
    "serviceId": "string",
    "lastName": "string",
    "firstName": "string",
    "birthDate": "2019-08-24T14:15:22Z",
    "email": "string",
    "createdAt": "2019-08-24T14:15:22Z",
    "lastUpdatedAt": "2019-08-24T14:15:22Z",
    "roleId": 0,
    "role": {
      "roleId": 0,
      "roleName": "string",
      "roleNumber": 0
    },
    "orders": [
      {
        "orderId": 0,
        "orderPrice": 0,
        "orderTaxPrice": 0,
        "orderDeliveryPrice": 0,
        "orderDate": "2019-08-24T14:15:22Z",
        "orderStatus": true,
        "userId": 0,
        "user": {},
        "products": [
          {
            "orderProductId": 0,
            "productId": 0,
            "product": {
              "productId": 0,
              "productBrandName": "string",
              "productName": "string",
              "productDescription": "string",
              "productPrice": 0,
              "productCreationDate": "2019-08-24T14:15:22Z",
              "productLastUpdate": "2019-08-24T14:15:22Z",
              "stockId": 0,
              "stock": {
                "stockId": 0,
                "stockQuantityAvailable": 0
              },
              "categories": [
                {
                  "categoryId": 0,
                  "categoryTitle": "string",
                  "categoryDescription": "string",
                  "products": [
                    {}
                  ]
                }
              ]
            },
            "orderId": 0,
            "order": {},
            "quantity": 0,
            "shipped": true
          }
        ],
        "animals": [
          {
            "animalId": 0,
            "animalBirthDate": "2019-08-24T14:15:22Z",
            "animalColor": "string",
            "animalPrice": 0,
            "animalCreationDate": "2019-08-24T14:15:22Z",
            "animalLastUpdate": "2019-08-24T14:15:22Z",
            "speciesId": 0,
            "species": {
              "speciesId": 0,
              "speciesName": "string"
            }
          }
        ]
      }
    ]
  },
  "products": [
    {
      "orderProductId": 0,
      "productId": 0,
      "product": {
        "productId": 0,
        "productBrandName": "string",
        "productName": "string",
        "productDescription": "string",
        "productPrice": 0,
        "productCreationDate": "2019-08-24T14:15:22Z",
        "productLastUpdate": "2019-08-24T14:15:22Z",
        "stockId": 0,
        "stock": {
          "stockId": 0,
          "stockQuantityAvailable": 0
        },
        "categories": [
          {
            "categoryId": 0,
            "categoryTitle": "string",
            "categoryDescription": "string",
            "products": [
              {}
            ]
          }
        ]
      },
      "orderId": 0,
      "order": {
        "orderId": 0,
        "orderPrice": 0,
        "orderTaxPrice": 0,
        "orderDeliveryPrice": 0,
        "orderDate": "2019-08-24T14:15:22Z",
        "orderStatus": true,
        "userId": 0,
        "user": {
          "userId": 0,
          "serviceId": "string",
          "lastName": "string",
          "firstName": "string",
          "birthDate": "2019-08-24T14:15:22Z",
          "email": "string",
          "createdAt": "2019-08-24T14:15:22Z",
          "lastUpdatedAt": "2019-08-24T14:15:22Z",
          "roleId": 0,
          "role": {
            "roleId": 0,
            "roleName": "string",
            "roleNumber": 0
          },
          "orders": [
            {}
          ]
        },
        "products": [],
        "animals": [
          {
            "animalId": 0,
            "animalBirthDate": "2019-08-24T14:15:22Z",
            "animalColor": "string",
            "animalPrice": 0,
            "animalCreationDate": "2019-08-24T14:15:22Z",
            "animalLastUpdate": "2019-08-24T14:15:22Z",
            "speciesId": 0,
            "species": {
              "speciesId": 0,
              "speciesName": "string"
            }
          }
        ]
      },
      "quantity": 0,
      "shipped": true
    }
  ],
  "animals": [
    {
      "animalId": 0,
      "animalBirthDate": "2019-08-24T14:15:22Z",
      "animalColor": "string",
      "animalPrice": 0,
      "animalCreationDate": "2019-08-24T14:15:22Z",
      "animalLastUpdate": "2019-08-24T14:15:22Z",
      "speciesId": 0,
      "species": {
        "speciesId": 0,
        "speciesName": "string"
      }
    }
  ]
}

```

### Properties

|Name|Type|Required|Restrictions|Description|
|---|---|---|---|---|
|orderId|integer(int32)|false|none|none|
|orderPrice|integer(int32)|false|none|none|
|orderTaxPrice|integer(int32)|false|none|none|
|orderDeliveryPrice|integer(int32)|false|none|none|
|orderDate|string(date-time)|false|none|none|
|orderStatus|boolean|false|none|none|
|userId|integer(int32)|false|none|none|
|user|[User](#schemauser)|false|none|none|
|products|[[OrderProduct](#schemaorderproduct)]¦null|false|none|none|
|animals|[[Animal](#schemaanimal)]¦null|false|none|none|

<h2 id="tocS_OrderProduct">OrderProduct</h2>
<!-- backwards compatibility -->
<a id="schemaorderproduct"></a>
<a id="schema_OrderProduct"></a>
<a id="tocSorderproduct"></a>
<a id="tocsorderproduct"></a>

```json
{
  "orderProductId": 0,
  "productId": 0,
  "product": {
    "productId": 0,
    "productBrandName": "string",
    "productName": "string",
    "productDescription": "string",
    "productPrice": 0,
    "productCreationDate": "2019-08-24T14:15:22Z",
    "productLastUpdate": "2019-08-24T14:15:22Z",
    "stockId": 0,
    "stock": {
      "stockId": 0,
      "stockQuantityAvailable": 0
    },
    "categories": [
      {
        "categoryId": 0,
        "categoryTitle": "string",
        "categoryDescription": "string",
        "products": [
          {}
        ]
      }
    ]
  },
  "orderId": 0,
  "order": {
    "orderId": 0,
    "orderPrice": 0,
    "orderTaxPrice": 0,
    "orderDeliveryPrice": 0,
    "orderDate": "2019-08-24T14:15:22Z",
    "orderStatus": true,
    "userId": 0,
    "user": {
      "userId": 0,
      "serviceId": "string",
      "lastName": "string",
      "firstName": "string",
      "birthDate": "2019-08-24T14:15:22Z",
      "email": "string",
      "createdAt": "2019-08-24T14:15:22Z",
      "lastUpdatedAt": "2019-08-24T14:15:22Z",
      "roleId": 0,
      "role": {
        "roleId": 0,
        "roleName": "string",
        "roleNumber": 0
      },
      "orders": [
        {}
      ]
    },
    "products": [
      {
        "orderProductId": 0,
        "productId": 0,
        "product": {
          "productId": 0,
          "productBrandName": "string",
          "productName": "string",
          "productDescription": "string",
          "productPrice": 0,
          "productCreationDate": "2019-08-24T14:15:22Z",
          "productLastUpdate": "2019-08-24T14:15:22Z",
          "stockId": 0,
          "stock": {
            "stockId": 0,
            "stockQuantityAvailable": 0
          },
          "categories": [
            {
              "categoryId": 0,
              "categoryTitle": "string",
              "categoryDescription": "string",
              "products": [
                {}
              ]
            }
          ]
        },
        "orderId": 0,
        "order": {},
        "quantity": 0,
        "shipped": true
      }
    ],
    "animals": [
      {
        "animalId": 0,
        "animalBirthDate": "2019-08-24T14:15:22Z",
        "animalColor": "string",
        "animalPrice": 0,
        "animalCreationDate": "2019-08-24T14:15:22Z",
        "animalLastUpdate": "2019-08-24T14:15:22Z",
        "speciesId": 0,
        "species": {
          "speciesId": 0,
          "speciesName": "string"
        }
      }
    ]
  },
  "quantity": 0,
  "shipped": true
}

```

### Properties

|Name|Type|Required|Restrictions|Description|
|---|---|---|---|---|
|orderProductId|integer(int32)|false|none|none|
|productId|integer(int32)|false|none|none|
|product|[Product](#schemaproduct)|false|none|Product model|
|orderId|integer(int32)|false|none|none|
|order|[Order](#schemaorder)|false|none|none|
|quantity|integer(int32)|false|none|none|
|shipped|boolean|false|none|none|

<h2 id="tocS_Product">Product</h2>
<!-- backwards compatibility -->
<a id="schemaproduct"></a>
<a id="schema_Product"></a>
<a id="tocSproduct"></a>
<a id="tocsproduct"></a>

```json
{
  "productId": 0,
  "productBrandName": "string",
  "productName": "string",
  "productDescription": "string",
  "productPrice": 0,
  "productCreationDate": "2019-08-24T14:15:22Z",
  "productLastUpdate": "2019-08-24T14:15:22Z",
  "stockId": 0,
  "stock": {
    "stockId": 0,
    "stockQuantityAvailable": 0
  },
  "categories": [
    {
      "categoryId": 0,
      "categoryTitle": "string",
      "categoryDescription": "string",
      "products": [
        {
          "productId": 0,
          "productBrandName": "string",
          "productName": "string",
          "productDescription": "string",
          "productPrice": 0,
          "productCreationDate": "2019-08-24T14:15:22Z",
          "productLastUpdate": "2019-08-24T14:15:22Z",
          "stockId": 0,
          "stock": {
            "stockId": 0,
            "stockQuantityAvailable": 0
          },
          "categories": []
        }
      ]
    }
  ]
}

```

Product model

### Properties

|Name|Type|Required|Restrictions|Description|
|---|---|---|---|---|
|productId|integer(int32)|false|none|none|
|productBrandName|string¦null|false|none|none|
|productName|string¦null|false|none|none|
|productDescription|string¦null|false|none|none|
|productPrice|integer(int32)|false|none|none|
|productCreationDate|string(date-time)|false|none|none|
|productLastUpdate|string(date-time)|false|none|none|
|stockId|integer(int32)|false|none|none|
|stock|[Stock](#schemastock)|false|none|none|
|categories|[[Category](#schemacategory)]¦null|false|none|none|

<h2 id="tocS_ProductDto">ProductDto</h2>
<!-- backwards compatibility -->
<a id="schemaproductdto"></a>
<a id="schema_ProductDto"></a>
<a id="tocSproductdto"></a>
<a id="tocsproductdto"></a>

```json
{
  "productId": 0,
  "productBrandName": "string",
  "productName": "string",
  "productDescription": "string",
  "productPrice": 0,
  "productCreationDate": "2019-08-24T14:15:22Z",
  "productLastUpdate": "2019-08-24T14:15:22Z",
  "stock": {
    "quantity": 0
  },
  "categories": [
    {
      "categoryTitle": "string",
      "categoryDescription": "string",
      "products": [
        {
          "productId": 0,
          "productBrandName": "string",
          "productName": "string",
          "productDescription": "string",
          "productPrice": 0,
          "productCreationDate": "2019-08-24T14:15:22Z",
          "productLastUpdate": "2019-08-24T14:15:22Z",
          "stock": {
            "quantity": 0
          },
          "categories": []
        }
      ]
    }
  ]
}

```

### Properties

|Name|Type|Required|Restrictions|Description|
|---|---|---|---|---|
|productId|integer(int32)|false|none|none|
|productBrandName|string¦null|false|none|none|
|productName|string¦null|false|none|none|
|productDescription|string¦null|false|none|none|
|productPrice|integer(int32)|false|none|none|
|productCreationDate|string(date-time)|false|none|none|
|productLastUpdate|string(date-time)|false|none|none|
|stock|[StockDto](#schemastockdto)|false|none|none|
|categories|[[CategoryDto](#schemacategorydto)]¦null|false|none|none|

<h2 id="tocS_Role">Role</h2>
<!-- backwards compatibility -->
<a id="schemarole"></a>
<a id="schema_Role"></a>
<a id="tocSrole"></a>
<a id="tocsrole"></a>

```json
{
  "roleId": 0,
  "roleName": "string",
  "roleNumber": 0
}

```

### Properties

|Name|Type|Required|Restrictions|Description|
|---|---|---|---|---|
|roleId|integer(int32)|false|none|none|
|roleName|string¦null|false|none|none|
|roleNumber|integer(int32)|false|none|none|

<h2 id="tocS_Species">Species</h2>
<!-- backwards compatibility -->
<a id="schemaspecies"></a>
<a id="schema_Species"></a>
<a id="tocSspecies"></a>
<a id="tocsspecies"></a>

```json
{
  "speciesId": 0,
  "speciesName": "string"
}

```

### Properties

|Name|Type|Required|Restrictions|Description|
|---|---|---|---|---|
|speciesId|integer(int32)|false|none|none|
|speciesName|string¦null|false|none|none|

<h2 id="tocS_Stock">Stock</h2>
<!-- backwards compatibility -->
<a id="schemastock"></a>
<a id="schema_Stock"></a>
<a id="tocSstock"></a>
<a id="tocsstock"></a>

```json
{
  "stockId": 0,
  "stockQuantityAvailable": 0
}

```

### Properties

|Name|Type|Required|Restrictions|Description|
|---|---|---|---|---|
|stockId|integer(int32)|false|none|none|
|stockQuantityAvailable|integer(int32)|false|none|none|

<h2 id="tocS_StockDto">StockDto</h2>
<!-- backwards compatibility -->
<a id="schemastockdto"></a>
<a id="schema_StockDto"></a>
<a id="tocSstockdto"></a>
<a id="tocsstockdto"></a>

```json
{
  "quantity": 0
}

```

### Properties

|Name|Type|Required|Restrictions|Description|
|---|---|---|---|---|
|quantity|integer(int32)|false|none|none|

<h2 id="tocS_User">User</h2>
<!-- backwards compatibility -->
<a id="schemauser"></a>
<a id="schema_User"></a>
<a id="tocSuser"></a>
<a id="tocsuser"></a>

```json
{
  "userId": 0,
  "serviceId": "string",
  "lastName": "string",
  "firstName": "string",
  "birthDate": "2019-08-24T14:15:22Z",
  "email": "string",
  "createdAt": "2019-08-24T14:15:22Z",
  "lastUpdatedAt": "2019-08-24T14:15:22Z",
  "roleId": 0,
  "role": {
    "roleId": 0,
    "roleName": "string",
    "roleNumber": 0
  },
  "orders": [
    {
      "orderId": 0,
      "orderPrice": 0,
      "orderTaxPrice": 0,
      "orderDeliveryPrice": 0,
      "orderDate": "2019-08-24T14:15:22Z",
      "orderStatus": true,
      "userId": 0,
      "user": {
        "userId": 0,
        "serviceId": "string",
        "lastName": "string",
        "firstName": "string",
        "birthDate": "2019-08-24T14:15:22Z",
        "email": "string",
        "createdAt": "2019-08-24T14:15:22Z",
        "lastUpdatedAt": "2019-08-24T14:15:22Z",
        "roleId": 0,
        "role": {
          "roleId": 0,
          "roleName": "string",
          "roleNumber": 0
        },
        "orders": []
      },
      "products": [
        {
          "orderProductId": 0,
          "productId": 0,
          "product": {
            "productId": 0,
            "productBrandName": "string",
            "productName": "string",
            "productDescription": "string",
            "productPrice": 0,
            "productCreationDate": "2019-08-24T14:15:22Z",
            "productLastUpdate": "2019-08-24T14:15:22Z",
            "stockId": 0,
            "stock": {
              "stockId": 0,
              "stockQuantityAvailable": 0
            },
            "categories": [
              {
                "categoryId": 0,
                "categoryTitle": "string",
                "categoryDescription": "string",
                "products": [
                  {}
                ]
              }
            ]
          },
          "orderId": 0,
          "order": {},
          "quantity": 0,
          "shipped": true
        }
      ],
      "animals": [
        {
          "animalId": 0,
          "animalBirthDate": "2019-08-24T14:15:22Z",
          "animalColor": "string",
          "animalPrice": 0,
          "animalCreationDate": "2019-08-24T14:15:22Z",
          "animalLastUpdate": "2019-08-24T14:15:22Z",
          "speciesId": 0,
          "species": {
            "speciesId": 0,
            "speciesName": "string"
          }
        }
      ]
    }
  ]
}

```

### Properties

|Name|Type|Required|Restrictions|Description|
|---|---|---|---|---|
|userId|integer(int32)|false|none|none|
|serviceId|string|true|none|none|
|lastName|string¦null|false|none|none|
|firstName|string¦null|false|none|none|
|birthDate|string(date-time)¦null|false|none|none|
|email|string|true|none|none|
|createdAt|string(date-time)|false|none|none|
|lastUpdatedAt|string(date-time)|false|none|none|
|roleId|integer(int32)|false|none|none|
|role|[Role](#schemarole)|false|none|none|
|orders|[[Order](#schemaorder)]¦null|false|none|none|

<h2 id="tocS_UserDto">UserDto</h2>
<!-- backwards compatibility -->
<a id="schemauserdto"></a>
<a id="schema_UserDto"></a>
<a id="tocSuserdto"></a>
<a id="tocsuserdto"></a>

```json
{
  "userId": 0,
  "lastName": "string",
  "firstName": "string",
  "email": "string"
}

```

### Properties

|Name|Type|Required|Restrictions|Description|
|---|---|---|---|---|
|userId|integer(int32)|false|none|none|
|lastName|string¦null|false|none|none|
|firstName|string¦null|false|none|none|
|email|string¦null|false|none|none|

<h2 id="tocS_UserRole">UserRole</h2>
<!-- backwards compatibility -->
<a id="schemauserrole"></a>
<a id="schema_UserRole"></a>
<a id="tocSuserrole"></a>
<a id="tocsuserrole"></a>

```json
{
  "roleName": "string",
  "token": "string"
}

```

### Properties

|Name|Type|Required|Restrictions|Description|
|---|---|---|---|---|
|roleName|string¦null|false|none|none|
|token|string¦null|false|none|none|

